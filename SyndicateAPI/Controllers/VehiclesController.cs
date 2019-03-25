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

namespace SyndicateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : Controller
    {
        private IUserService UserService { get; set; }
        private IFileService FileService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehicleClassService VehicleClassService { get; set; }
        private IVehicleCategoryService VehicleCategoryService { get; set; }

        public VehiclesController([FromServices]
            IUserService userService,
            IFileService fileService,
            IVehicleService vehicleService,
            IVehicleClassService vehicleClassService,
            IVehicleCategoryService vehicleCategoryService)
        {
            UserService = userService;
            FileService = fileService;
            VehicleService = vehicleService;
            VehicleClassService = vehicleClassService;
            VehicleCategoryService = vehicleCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateUpdateVehicleRequest request)
        {
            var vehicleClass = VehicleClassService.Get(request.ClassID);
            if (vehicleClass == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Class ID invalid"
                });

            var vehicleCategory = VehicleCategoryService.Get(request.CategoryID);
            if (vehicleCategory == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Category ID invalid"
                });

            var photo = FileService.Get(request.PhotoID);
            if (photo == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Photo ID invalid"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            var vehicle = new Vehicle
            {
                Model = request.Model,
                Power = request.Power,
                Year = request.Year,
                Price = request.Price,
                Photo = photo,
                Class = vehicleClass,
                Category = vehicleCategory,
                Drive = request.Drive,
                Transmission = request.Transmission,
                Body = request.Body,
                Owner = user
            };

            VehicleService.Create(vehicle);

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = new VehicleViewModel(vehicle)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle([FromBody] CreateUpdateVehicleRequest request, long id)
        {
            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Vehicle ID invalid"
                });

            var user = UserService.GetAll().FirstOrDefault(x => x.Login == User.Identity.Name);
            if (vehicle.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "You are not owner of this vehicle"
                });

            if (request.ClassID != vehicle.Class.ID)
            {
                var vehicleClass = VehicleClassService.Get(request.ClassID);
                if (vehicleClass == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Class ID invalid"
                    });

                vehicle.Class = vehicleClass;
            }

            if (request.CategoryID != vehicle.Category.ID)
            {
                var vehicleCategory = VehicleCategoryService.Get(request.CategoryID);
                if (vehicleCategory == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Category ID invalid"
                    });

                vehicle.Category = vehicleCategory;
            }

            if (request.PhotoID != vehicle.Photo.ID)
            {
                var photo = FileService.Get(request.PhotoID);
                if (photo == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Photo ID invalid"
                    });

                vehicle.Photo = photo;
            }

            if (request.Model != vehicle.Model)
                vehicle.Model = request.Model;

            if (request.Power != vehicle.Power)
                vehicle.Power = request.Power;

            if (request.Year != vehicle.Year)
                vehicle.Year = request.Year;

            if (request.Price != vehicle.Price)
                vehicle.Price = request.Price;

            if (request.Drive != vehicle.Drive)
                vehicle.Drive = request.Drive;

            if (request.Transmission != vehicle.Transmission)
                vehicle.Transmission = request.Transmission;

            if (request.Body != vehicle.Body)
                vehicle.Body = request.Body;

            VehicleService.Update(vehicle);

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = new VehicleViewModel(vehicle)
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(long id)
        {
            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Vehicle ID invalid"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (vehicle.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "You are not owner of this vehicle"
                });

            VehicleService.Delete(id);

            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .Select(x => new VehicleViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .Select(x => new VehicleViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
            });
        }

        [HttpGet("{id}/vehicles")]
        public async Task<IActionResult> GetVehicles(long id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .Select(x => new VehicleViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
            });
        }
    }
}