using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : Controller
    {
        private ICityService CityService { get; set; }

        public CitiesController([FromServices] ICityService cityService)
        {
            CityService = cityService;
        }

        [HttpPost("createStartData")]
        public async Task<IActionResult> CreateStartData()
        {
            var cities = new List<City>()
            {
                new City { Name = "Москва" },
                new City { Name = "Санкт-Петербург" },
                new City { Name = "Белгород" },
                new City { Name = "Воронеж" },
                new City { Name = "Ростов-на-Дону" },
                new City { Name = "Ставрополь" },
                new City { Name = "Тула" },
                new City { Name = "Калуга" },
                new City { Name = "Казань" },
                new City { Name = "Екатеринбург" },
                new City { Name = "Нижний Новгород" },
                new City { Name = "Новосибирск" },
                new City { Name = "Ульяновск" },
                new City { Name = "Владивосток" },
                new City { Name = "Калининград" }
            };

            foreach (var city in cities)
                CityService.Create(city);

            return Ok(new ResponseModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = CityService.GetAll()
                .Select(x => new CityViewModel(x))
                .ToList();

            return Json(new DataResponse<List<CityViewModel>>
            {
                Data = cities
            });
        }
    }
}