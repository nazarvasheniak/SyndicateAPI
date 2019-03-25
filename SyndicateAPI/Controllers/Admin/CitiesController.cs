using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/admin/cities")]
    [ApiController]
    [Authorize]
    public class CitiesController : Controller
    {
        private IAdminUserService AdminUserService { get; set; }
        private ICityService CityService { get; set; }

        public CitiesController([FromServices]
            IAdminUserService adminUserService,
            ICityService cityService)
        {
            AdminUserService = adminUserService;
            CityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var cities = CityService.GetAll()
                .Select(x => new CityViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<CityViewModel>>
            {
                Data = cities
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(long id)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var city = CityService.Get(id);
            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City not found"
                });

            return Ok(new DataResponse<CityViewModel>
            {
                Data = new CityViewModel(city)
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCity(string name)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var city = CityService.GetAll()
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City not found"
                });

            return Ok(new DataResponse<CityViewModel>
            {
                Data = new CityViewModel(city)

            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
        {
            var user = AdminUserService.GetAll()
                            .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var city = CityService.GetAll()
                .FirstOrDefault(x => x.Name.ToLower() == request.Name.ToLower());

            if (city != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City already exist"
                });

            city = new City { Name = request.Name };
            CityService.Create(city);

            return Ok(new DataResponse<CityViewModel>
            {
                Data = new CityViewModel(city)
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(long id)
        {
            var user = AdminUserService.GetAll()
                            .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var city = CityService.Get(id);
            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City not found"
                });

            CityService.Delete(id);

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCity(string name)
        {
            var user = AdminUserService.GetAll()
                            .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var city = CityService.GetAll()
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City not found"
                });

            CityService.Delete(city.ID);

            return Ok();
        }
    }
}