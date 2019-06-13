using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Admin;
using SyndicateAPI.Models.Response;
using System.Linq;
using System.Threading.Tasks;

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/admin/vehicles")]
    [ApiController]
    [Authorize]
    public class VehiclesController : Controller
    {
        private IAdminUserService AdminUserService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehiclePhotoService VehiclePhotoService { get; set; }

        public VehiclesController([FromServices] 
            IAdminUserService adminUserService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService)
        {
            AdminUserService = adminUserService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles([FromQuery] GetVehiclesRequest request)
        {
            var vehicles = VehicleService.GetAll()
                .Where(x => x.ApproveStatus == request.Status)
                .ToList();

            var result = vehicles
                .Skip((request.PageNumber - 1) * request.PageCount)
                .Take(request.PageCount)
                .Select(x => new VehicleViewModel(x))
                .ToList();

            return Ok(new ListResponse<VehicleViewModel>
            {
                Data = result,
                Pagination = new Pagination(vehicles.Count, request.PageNumber, request.PageCount)
            });
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(long id)
        {
            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Автомобиль не найден"
                });

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = VehicleToViewModel(vehicle)
            });
        }

        private VehicleViewModel VehicleToViewModel(Vehicle vehicle)
        {
            var photos = VehiclePhotoService.GetAll()
                .Where(x => x.Vehicle == vehicle)
                .ToList();

            var result = new VehicleViewModel(vehicle, photos);

            return result;
        }
    }
}