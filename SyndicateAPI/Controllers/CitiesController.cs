using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : Controller
    {
        private ICityService CityService { get; set; }

        public CitiesController([FromServices] ICityService cityService)
        {
            CityService = cityService;
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