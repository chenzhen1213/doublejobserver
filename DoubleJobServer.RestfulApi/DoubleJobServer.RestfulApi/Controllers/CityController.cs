using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DoubleJobServer.Core.Interfaces;
using AutoMapper;
using DoubleJobServer.Infrastructure.Resources;
using DoubleJobServer.Core.DomainModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace DoubleJobServer.RestfulApi.Controllers
{
    [Route("api/countries/{countryId}/cities")]
    public class CityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CityController> _logger;

        //ILogger<T>，T就是日志分类的名字，这里建议使用Controller的名字
        public CityController(IUnitOfWork unitOfWork,
            ICountryRepository countryRepository,
            ICityRepository cityRepository,
            IMapper mapper,
            ILogger<CityController> logger)
        {
            _unitOfWork = unitOfWork;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesForCountry(int countryId)
        {
            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }

            var citiesForCountry = await _cityRepository.GetCitiesForCountryAsync(countryId);
            var citiesResources = _mapper.Map<IEnumerable<CityResource>>(citiesForCountry);
            return Ok(citiesResources);
        }

        [HttpGet("{cityId}", Name = "GetCity")]
        public async Task<IActionResult> GetCityForCountry(int countryId, int cityId)
        {
            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                _logger.LogInformation("Not Found");
                return NotFound();
            }

            var cityForCountry = await _cityRepository.GetCityForCountryAsync(countryId, cityId);

            if (cityForCountry == null)
            {
                return NotFound();
            }

            var cityResource = _mapper.Map<CityResource>(cityForCountry);
            return Ok(cityResource);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCityForCountry(int countryId, [FromBody] CityAddResource city)
        {
            if (city == null)
            {
                return BadRequest();
            }

            //直接在方法里写验证逻辑 要求City的name属性值不可以是“中国”
            if (city.Name == "中国")
            {
                ModelState.AddModelError(nameof(city.Name), "城市的名称不可以叫中国");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }

            var cityModel = _mapper.Map<City>(city);
            _cityRepository.AddCityForCountry(countryId, cityModel);

            if (!await _unitOfWork.SaveAsync())
            {
                return StatusCode(500, "Error occurred when adding");
            }

            var cityResource = _mapper.Map<CityResource>(cityModel);
            return CreatedAtRoute("GetCity", new { countryId, cityId = cityModel.Id }, cityResource);
        }


        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCityForCountry(int countryId, int cityId)
        {
            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                return NotFound();
            }

            _cityRepository.DeleteCity(city);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting city {cityId} for country {countryId} failed when saving.");
            }

            return NoContent();
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCityForCountry(int countryId, int cityId,
          [FromBody] CityUpdateResource cityUpdate)
        {
            if (cityUpdate == null)
            {
                return BadRequest();
            }

            if (cityUpdate.Name =="中国")
            {
                ModelState.AddModelError(nameof(cityUpdate.Name), "城市的名称不可以叫中国");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                var cityToAdd = _mapper.Map<City>(cityUpdate);
                cityToAdd.Id = cityId; // 如果Id不是自增的话
                _cityRepository.AddCityForCountry(countryId, cityToAdd);

                if (!await _unitOfWork.SaveAsync())
                {
                    return StatusCode(500, $"Upserting city {cityId} for country {countryId} failed when inserting");
                }

                var cityResource = Mapper.Map<CityResource>(cityToAdd);
                return CreatedAtRoute("GetCityForCountry", new { countryId, cityId }, cityResource);
                //return NotFound();
            }

            // 把cityUpdate的属性值都映射给city 把第一个参数对象的属性映射到第二个参数对象上
            _mapper.Map(cityUpdate, city);

            _cityRepository.UpdateCityForCountry(city);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating city {cityId} for country {countryId} failed when saving.");
            }

            //Ok和NoContent都是可以的，如果在Action的方法里某些属性的值是在这里改变的，那么可以使用Ok把最新的对象传递回去；但是如果在Action方法里没有再修改其它属性的值，也就是说更新之后和传递进来的对象的属性值是一样的，那就没有必要再把最新的对象传递回去了，这时就应该使用NoContent。
            return NoContent();
        }


        [HttpPatch("{cityId}")]
        public async Task<IActionResult> PartiallyUpdateCityForCountry(int countryId, int cityId,
    [FromBody] JsonPatchDocument<CityUpdateResource> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!await _countryRepository.CountryExistAsync(countryId))
            {
                return NotFound();
            }

            var city = await _cityRepository.GetCityForCountryAsync(countryId, cityId);
            if (city == null)
            {
                var cityUpdate = new CityUpdateResource();
                patchDoc.ApplyTo(cityUpdate);
                var cityToAdd = _mapper.Map<City>(cityUpdate);
                cityToAdd.Id = cityId; // 只适用于Id不是自增的情况

                _cityRepository.AddCityForCountry(countryId, cityToAdd);

                if (!await _unitOfWork.SaveAsync())
                {
                    return StatusCode(500, $"P city {cityId} for country {countryId} failed when inserting");
                }
                var cityResource = Mapper.Map<CityResource>(cityToAdd);
                return CreatedAtRoute("GetCityForCountry", new { countryId, cityId }, cityResource);
                //return NotFound();
            }

            //把EFCore的City映射成CityUpdateResource，这样这个CityUpdateResource就有了该City在数据库里最新的属性值
            var cityToPatch = _mapper.Map<CityUpdateResource>(city);

            //patchDoc里面有任何验证错误都会在ModelState里面体现出来
            //patchDoc.ApplyTo(cityToPatch, ModelState);
            patchDoc.ApplyTo(cityToPatch);

            TryValidateModel(cityToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map(cityToPatch, city);
            _cityRepository.UpdateCityForCountry(city);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Patching city {cityId} for country {countryId} failed when saving.");
            }

            return NoContent();
        }
    }
}