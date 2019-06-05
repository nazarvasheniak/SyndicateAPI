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
        private IStartRewardService StartRewardService { get; set; }

        public VehiclesController([FromServices]
            IUserService userService,
            IFileService fileService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService,
            IVehicleCategoryService vehicleCategoryService,
            IVehicleDriveService vehicleDriveService,
            IVehicleTransmissionService vehicleTransmissionService,
            IVehicleBodyService vehicleBodyService,
            IStartRewardService startRewardService)
        {
            UserService = userService;
            FileService = fileService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
            VehicleCategoryService = vehicleCategoryService;
            VehicleDriveService = vehicleDriveService;
            VehicleTransmissionService = vehicleTransmissionService;
            VehicleBodyService = vehicleBodyService;
            StartRewardService = startRewardService;
        }

        [HttpPost("createStartData")]
        public async Task<IActionResult> CreateStartData()
        {
            var categories = new List<VehicleCategory>()
            {
                new VehicleCategory { Title = "Жоповоз" },
                new VehicleCategory { Title = "Стиль" },
                new VehicleCategory { Title = "Дрифт" },
                new VehicleCategory { Title = "Драг" },
                new VehicleCategory { Title = "Ведро" },
                new VehicleCategory { Title = "Оффроад" },
                new VehicleCategory { Title = "Бусик" },
                new VehicleCategory { Title = "Ралли" },
                new VehicleCategory { Title = "Дерби" },
                new VehicleCategory { Title = "Мото" }
            };

            foreach (var category in categories)
                VehicleCategoryService.Create(category);

            var drives = new List<VehicleDrive>()
            {
                new VehicleDrive { Title = "Отсутствует" },
                new VehicleDrive { Title = "Передний" },
                new VehicleDrive { Title = "Задний" },
                new VehicleDrive { Title = "Полный" }
            };

            foreach (var drive in drives)
                VehicleDriveService.Create(drive);

            var transmissions = new List<VehicleTransmission>()
            {
                new VehicleTransmission { Title = "Отсутствует" },
                new VehicleTransmission { Title = "Механическая" },
                new VehicleTransmission { Title = "Автоматизированная" },
                new VehicleTransmission { Title = "Роботизированная" },
                new VehicleTransmission { Title = "Вариаторная" }
            };

            foreach (var transmission in transmissions)
                VehicleTransmissionService.Create(transmission);

            var bodies = new List<VehicleBody>()
            {
                new VehicleBody { Title = "Отсутствует" },
                new VehicleBody { Title = "Седан" },
                new VehicleBody { Title = "Универсал" },
                new VehicleBody { Title = "Хэтчбэк" },
                new VehicleBody { Title = "Купе" },
                new VehicleBody { Title = "Лимузин" },
                new VehicleBody { Title = "Микроавтобус" },
                new VehicleBody { Title = "Минивэн" },
                new VehicleBody { Title = "Хардтоп" },
                new VehicleBody { Title = "Таун-Кар" },
                new VehicleBody { Title = "Комби" },
                new VehicleBody { Title = "Лифтбэк" },
                new VehicleBody { Title = "Фастбэк" },
                new VehicleBody { Title = "Кабриолет" },
                new VehicleBody { Title = "Родстер" },
                new VehicleBody { Title = "Фаэтон" },
                new VehicleBody { Title = "Ландо" },
                new VehicleBody { Title = "Брогам" },
                new VehicleBody { Title = "Тарга" },
                new VehicleBody { Title = "Спайдер" },
                new VehicleBody { Title = "Шутингбрейк" },
                new VehicleBody { Title = "Пикап" },
                new VehicleBody { Title = "Фургон" }
            };

            foreach (var body in bodies)
                VehicleBodyService.Create(body);

            return Ok(new ResponseModel());
        }

        [HttpPost]
        [Authorize]
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

            var result = new CreateVehicleResponse { Data = VehicleToViewModel(vehicle) };

            var startReward = StartRewardService.GetAll().FirstOrDefault(x => x.User == user);
            if (!startReward.IsVehicleCompleted)
            {
                result.BonusPoints += 40;
                result.Message = $"Вам начислено {result.BonusPoints} за добавление авто в гараж";

                user.PointsCount += 40;
                UserService.Update(user);

                startReward.IsVehicleCompleted = true;
                StartRewardService.Update(startReward);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
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

            if (request.PhotoList != null && request.PhotoList.Count != 0)
            {
                var existPhotos = VehiclePhotoService.GetAll()
                    .Where(x => x.Vehicle == vehicle)
                    .ToList();
                
                foreach (var photo in existPhotos)
                    VehiclePhotoService.Delete(photo);

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
                Data = VehicleToViewModel(vehicle)
            });
        }

        [HttpDelete("{id}")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetVehicles()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .Select(x => VehicleToViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
            });
        }
        
        [HttpGet("categories")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        [HttpGet("unapproved")]
        [Authorize]
        public async Task<IActionResult> GetUnapprovedVehicles()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.PointsCount < 10001)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Действие доступно только для Синдиката"
                });

            var vehicles = VehicleService.GetAll()
                .Where(x => x.ApproveStatus == VehicleApproveStatus.NotApproved)
                .Select(x => VehicleToViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
            });
        }

        [HttpPost("{id}/approve")]
        [Authorize]
        public async Task<IActionResult> ApproveVehicle(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.PointsCount < 10001)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Действие доступно только для Синдиката"
                });

            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Автомобиль не найден"
                });

            vehicle.ApproveStatus = VehicleApproveStatus.Approved;
            VehicleService.Update(vehicle);

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = VehicleToViewModel(vehicle)
            });
        }

        [HttpPost("{id}/decline")]
        [Authorize]
        public async Task<IActionResult> DeclineVehicle(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.PointsCount < 10001)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Действие доступно только для Синдиката"
                });

            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Автомобиль не найден"
                });

            vehicle.ApproveStatus = VehicleApproveStatus.Declined;
            VehicleService.Update(vehicle);

            return Ok(new DataResponse<VehicleViewModel>
            {
                Data = VehicleToViewModel(vehicle)
            });
        }

        [HttpPut("{id}/approve/photo")]
        [Authorize]
        public async Task<IActionResult> UpdateConfirmationPhoto([FromBody] UpdateVehicleConfirmPhotoRequest request, long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var vehicle = VehicleService.Get(id);
            if (vehicle == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Автомобиль не найден"
                });

            if (vehicle.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете редактировать чужой автомобиль"
                });

            if (request.PhotoID == 0)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "ID фото не может быть 0"
                });

            var photo = FileService.Get(request.PhotoID);
            if (photo == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Фото не найдено"
                });

            vehicle.ConfirmationPhoto = photo;
            VehicleService.Update(vehicle);

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