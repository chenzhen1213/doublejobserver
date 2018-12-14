using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DoubleJobServer.Infrastructure;
using AutoMapper;
using DoubleJobServer.Infrastructure.Resources;
using Microsoft.EntityFrameworkCore;
using DoubleJobServer.Infrastructure.Repositories;
using DoubleJobServer.Core.Interfaces;
using DoubleJobServer.Infrastructure;
using DoubleJobServer.Core.DomainModels;

namespace DoubleJobServer.RestfulApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Values")]
    public class ValuesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //private readonly MyContext _myContext;
        private readonly ICountryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ValuesController(/*MyContext myContext,*/IUnitOfWork unitOfWork, ICountryRepository repository, IMapper mapper)
        {
            //_myContext = myContext;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpGet]
        //         public IEnumerable<object> Get()
        //         {
        //             var data = _myContext.Countries.ToList();
        //             return data;
        //         }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var newCountry = new Country
            {
                ChineseName = "俄罗斯",
                EnglishName = "Russia",
                Abbreviation = "Russia"
            };

            _repository.AddCountry(newCountry);
            await _unitOfWork.SaveAsync();

            // var countries = await _myContext.Countries.ToListAsync();
            var countries = await _repository.GetCountriesAsync();
            var countryResources = _mapper.Map<List<CountryResource>>(countries);
            return Ok(countryResources);
        }
    }
}