using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    [Authorize]
    public class VehiclesController : Controller
    {
        private IUserService UserService { get; set; }
        private IFileService FileService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehiclePhotoService VehiclePhotoService { get; set; }
        private IVehicleCategoryService VehicleCategoryService { get; set; }
        private IVehicleDriveService VehicleDriveService { get; set; }
        private IVehicleTransmissionService VehicleTransmissionService { get; set; }
        private IVehicleBodyService VehicleBodyService { get; set; }

        public VehiclesController([FromServices]
            IUserService userService,
            IFileService fileService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService,
            IVehicleCategoryService vehicleCategoryService,
            IVehicleDriveService vehicleDriveService,
            IVehicleTransmissionService vehicleTransmissionService,
            IVehicleBodyService vehicleBodyService)
        {
            UserService = userService;
            FileService = fileService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
            VehicleCategoryService = vehicleCategoryService;
            VehicleDriveService = vehicleDriveService;
            VehicleTransmissionService = vehicleTransmissionService;
            VehicleBodyService = vehicleBodyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateUpdateVehicleRequest request)
        {
            var vehicleCategory = VehicleCategoryService.Get(request.CategoryID);
            if (vehicleCategory == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Category ID invalid"
                });

            var vehicleDrive = VehicleDriveService.Get(request.DriveID);
            if (vehicleDrive == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Drive ID invalid"
                });

            var vehicleTransmission = VehicleTransmissionService.Get(request.TransmissionID);
            if (vehicleTransmission == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Transmission ID invalid"
                });

            var vehicleBody = VehicleBodyService.Get(request.BodyID);
            if (vehicleTransmission == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Body ID invalid"
                });

            var confirmationPhoto = FileService.Get(request.ConfirmationPhotoID);
            if (confirmationPhoto == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Confirmation photo ID invalid"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var vehicle = new Vehicle
            {
                Model = request.Model,
                Power = request.Power,
                Year = request.Year,
                Price = request.Price,
                Category = vehicleCategory,
                Drive = vehicleDrive,
                Transmission = vehicleTransmission,
                Body = vehicleBody,
                Owner = user,
                ConfirmationPhoto = confirmationPhoto,
                ApproveStatus = VehicleApproveStatus.NotApproved
            };

            VehicleService.Create(vehicle);

            if (request.PhotoList != null && request.PhotoList.Count != 0)
            {
                if (request.PhotoList.Count > 10)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Max photo count is 10"
                    });

                foreach (var photoID in request.PhotoList)
                {
                    var file = FileService.Get(photoID);
                    if (file == null)
                        return BadRequest(new ResponseModel
                        {
                            Success = false,
                            Message = "Photo ID invalid"
                        });

                    var photo = new VehiclePhoto
                    {
                        Photo = file,
                        Vehicle = vehicle
                    };

                    VehiclePhotoService.Create(photo);
                }
            }

            var photos = VehiclePhotoService.GetAll()
                .Where(x => x.Vehicle == vehicle)
                .ToList();

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = new VehicleViewModel(vehicle, photos)
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

            var user = UserService.GetAll().FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);
            if (vehicle.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "You are not owner of this vehicle"
                });

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

            if (request.DriveID != vehicle.Drive.ID)
            {
                var vehicleDrive = VehicleDriveService.Get(request.DriveID);
                if (vehicleDrive == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Drive ID invalid"
                    });

                vehicle.Drive = vehicleDrive;
            }

            if (request.TransmissionID != vehicle.Transmission.ID)
            {
                var vehicleTransmission = VehicleTransmissionService.Get(request.TransmissionID);
                if (vehicleTransmission == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Transmission ID invalid"
                    });

                vehicle.Transmission = vehicleTransmission;
            }

            if (request.BodyID != vehicle.Body.ID)
            {
                var vehicleBody = VehicleBodyService.Get(request.BodyID);
                if (vehicleBody == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Body ID invalid"
                    });

                vehicle.Body = vehicleBody;
            }

            //if (request.PhotoList != null && request.PhotoList.Count != 0)
            //{
            //    foreach (var photoID in request.PhotoList)
            //    {
            //        var file = FileService.Get(photoID);
            //        if (file == null)
            //            return BadRequest(new ResponseModel
            //            {
            //                Success = false,
            //                Message = "Photo ID invalid"
            //            });

            //        var photo = new VehiclePhoto
            //        {
            //            Photo = file,
            //            Vehicle = vehicle
            //        };

            //        VehiclePhotoService.Create(photo);
            //    }
            //}

            if (request.ConfirmationPhotoID != vehicle.ConfirmationPhoto.ID)
            {
                var confirmationPhoto = FileService.Get(request.ConfirmationPhotoID);
                if (confirmationPhoto == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Confirmation photo ID invalid"
                    });

                vehicle.ConfirmationPhoto = confirmationPhoto;
            }

            if (request.Model != vehicle.Model)
                vehicle.Model = request.Model;

            if (request.Power != vehicle.Power)
                vehicle.Power = request.Power;

            if (request.Year != vehicle.Year)
                vehicle.Year = request.Year;

            if (request.Price != vehicle.Price)
                vehicle.Price = request.Price;

            vehicle.ApproveStatus = VehicleApproveStatus.NotApproved;

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
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

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
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .ToList();

            var data = new List<VehicleViewModel>();
            foreach (var vehicle in vehicles)
            {
                var photos = VehiclePhotoService.GetAll()
                    .Where(x => x.Vehicle == vehicle)
                    .ToList();

                data.Add(new VehicleViewModel(vehicle, photos));
            }

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = data
            });
        }
        
        [HttpGet("categories")]
        public async Task<IActionResult> GetVehicleCategories()
        {
            var categories = VehicleCategoryService.GetAll()
                .Select(x => new VehicleCategoryViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleCategoryViewModel>>
            {
                Data = categories
            });
        }

        [HttpGet("drives")]
        public async Task<IActionResult> GetVehicleDrives()
        {
            var drives = VehicleDriveService.GetAll()
                .Select(x => new VehicleDriveViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleDriveViewModel>>
            {
                Data = drives
            });
        }

        [HttpGet("transmissions")]
        public async Task<IActionResult> GetVehicleTransmissions()
        {
            var transmissions = VehicleTransmissionService.GetAll()
                .Select(x => new VehicleTransmissionViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleTransmissionViewModel>>
            {
                Data = transmissions
            });
        }

        [HttpGet("bodies")]
        public async Task<IActionResult> GetVehicleBodies()
        {
            var bodies = VehicleBodyService.GetAll()
                .Select(x => new VehicleBodyViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleBodyViewModel>>
            {
                Data = bodies
            });
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetVehicleProperties()
        {
            var categories = VehicleCategoryService.GetAll()
                .Select(x => new VehicleCategoryViewModel(x))
                .ToList();

            var drives = VehicleDriveService.GetAll()
                .Select(x => new VehicleDriveViewModel(x))
                .ToList();

            var transmissions = VehicleTransmissionService.GetAll()
                .Select(x => new VehicleTransmissionViewModel(x))
                .ToList();

            var bodies = VehicleBodyService.GetAll()
                .Select(x => new VehicleBodyViewModel(x))
                .ToList();

            return Ok(new DataResponse<VehicleProperties>
            {
                Data = new VehicleProperties
                {
                    Categories = categories,
                    Drives = drives,
                    Transmissions = transmissions,
                    Bodies = bodies
                }
            });
        }
    }
}