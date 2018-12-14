using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DoubleJobServer.Core.Interfaces;
using AutoMapper;
using DoubleJobServer.Infrastructure.Resources;
using DoubleJobServer.Core.DomainModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DoubleJobServer.RestfulApi.Controllers
{
    [Route("api/countries")]
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public CountryController(IUnitOfWork unitOfWork,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var countries = await _countryRepository.GetCountriesAsync();
                var countryResoureces = _mapper.Map<List<CountryResource>>(countries);
                return Ok(countryResoureces);  //Controller里提供了一些帮助方法返回IActionResult并指定特定的状态码，针对200，就是Ok()方法
            }
            catch (Exception e)
            {
                return StatusCode(500, "An Error Occurred");
            }
        }

        [HttpGet("{id}", Name = "GetCountry")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            var countryResource = _mapper.Map<CountryResource>(country);
            return new JsonResult(countryResource);

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> BlockCreatingCountry(int id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status409Conflict);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromBody] CountryAddResource country)
        {
            if (country == null)
            {
                return BadRequest();
            }

            var countryModel = _mapper.Map<Country>(country);
            _countryRepository.AddCountry(countryModel);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Error occurred when adding");
            }

            var countryResource = _mapper.Map<CountryResource>(countryModel);

            return CreatedAtRoute("GetCountry", new { id = countryModel.Id }, countryResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countryRepository.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _countryRepository.DeleteCountry(country);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting country {id} failed when saving.");
            }

            return NoContent();
        }


        [HttpPut("{id}", Name = "UpdateCountry")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryUpdateResource countryUpdate)
        {
            if (countryUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            var country = await _countryRepository.GetCountryByIdAsync(id, includeCities: true);
            if (country == null)
            {
                return NotFound();
            }

            /*
            //集合更新，我一共分了三步进行的操作：
            //1.把数据库中存在的但是传进来的数据里没有的城市删掉
            //2.把数据库中没有的而传进来的数据里有的数据进行添加操作，其实这里只判断id为0即可
            //3.把数据库中原有和传进来的参数里也存在的数据条目进行更新。

            //Remove
            var countryUpdateCityIds = countryUpdate.Cities.Select(x => x.Id).ToList();
            var removedCities = country.Cities.Where(c => !countryUpdateCityIds.Contains(c.Id)).ToList();

            foreach (var city in removedCities)
                country.Cities.Remove(city);

            //Add
            var addedCityResources = countryUpdate.Cities.Where(x => x.Id == 0);
            var addedCities = _mapper.Map<IEnumerable<City>>(addedCityResources);
            foreach (var city in addedCities)
            {
                country.Cities.Add(city);
            }

            //Update or Unchanged
            var maybeUpdateCities = country.Cities.Where(x => x.Id != 0).ToList();
            foreach (var city in maybeUpdateCities)
            {
                var cityResource = countryUpdate.Cities.Single(x => x.Id == city.Id);
                _mapper.Map(cityResource, city);
            }
            */

            _mapper.Map(countryUpdate, country);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating country {id} failed when saving.");
            }
            return NoContent();
        }

        private string CreateCountryUri(CountryResourceParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        chineseName = parameters.ChineseName,
                        englishName = parameters.EnglishName
                    };
                    return _urlHelper.Link("GetCountries", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        chineseName = parameters.ChineseName,
                        englishName = parameters.EnglishName
                    };
                    return _urlHelper.Link("GetCountries", nextParameters);
                default:
                case PaginationResourceUriType.CurrentPage:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        chineseName = parameters.ChineseName,
                        englishName = parameters.EnglishName
                    };
                    return _urlHelper.Link("GetCountries", currentParameters);
            }
        }

        [HttpGet(Name = "GetCountries")]
        //首先我们需要从参数(query string参数)传进来pageIndex和pageSize，还要赋默认值，以防止API的消费者没有设置pageIndex和pageSize；由于pageSize的值是由API的消费者来定的，所以应该在后端设定一个最大值，以免API的消费者设定一个很大的值
        //public async Task<IActionResult> Get([FromQuery] int pageIndex =0, int pageSize = 10)
        public async Task<IActionResult> GetCountries(CountryResourceParameters countryResourceParameters)
        {
            var pagedList = await _countryRepository.GetCountriesAsync(countryResourceParameters);
            var countryResources = _mapper.Map<List<CountryResource>>(pagedList);

            var previousPageLink = pagedList.HasPrevious ?
                 CreateCountryUri(countryResourceParameters, PaginationResourceUriType.PreviousPage) : null;

            var nextPageLink = pagedList.HasNext ?
                CreateCountryUri(countryResourceParameters, PaginationResourceUriType.NextPage) : null;


            var meta = new
            {
                pagedList.TotalItemsCount,
                pagedList.PaginationBase.PageSize,
                pagedList.PaginationBase.PageIndex,
                pagedList.PageCount,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta));

            return Ok(countryResources);
        }


    }
}