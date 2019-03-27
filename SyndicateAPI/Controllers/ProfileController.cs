using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserService UserService { get; set; }
        private IUserTempService UserTempService { get; set; }
        private IEmailService EmailService { get; set; }
        private IPersonService PersonService { get; set; }
        private IRewardService RewardService { get; set; }
        private ICityService CityService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }

        public ProfileController([FromServices]
            IUserService userService,
            IUserTempService userTempService,
            IEmailService emailService,
            IPersonService personService,
            IRewardService rewardService,
            ICityService cityService,
            IVehicleService vehicleService,
            IUserSubscriptionService userSubscriptionService)
        {
            UserService = userService;
            UserTempService = userTempService;
            PersonService = personService;
            RewardService = rewardService;
            CityService = cityService;
            VehicleService = vehicleService;
            UserSubscriptionService = userSubscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var profile = GetProfileModel(user);
            
            return Ok(new DataResponse<ProfileViewModel>
            {
                Data = profile
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(long id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            var profile = GetProfileModel(user);

            return Ok(new DataResponse<ProfileViewModel>
            {
                Data = profile
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (request.FirstName != null && !request.FirstName.Equals(user.Person.FirstName))
                user.Person.FirstName = request.FirstName;

            if (request.LastName != null && !request.LastName.Equals(user.Person.LastName))
                user.Person.LastName = request.LastName;

            if (request.Nickname != null && !request.Nickname.Equals(user.Nickname))
                user.Nickname = request.Nickname;

            if (request.CityID != 0 && !request.CityID.Equals(user.Person.City.ID))
            {
                var city = CityService.Get(request.CityID);
                if (city == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "City error"
                    });

                user.Person.City = city;
            }

            if (request.Biography != null && !request.Biography.Equals(user.Person.Biography))
                user.Person.Biography = request.Biography;

            if (request.Password != null && !request.Password.Equals(user.Password))
            {
                if (!request.Password.Equals(request.ConfirmPassword))
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Passwords are not equals"
                    });

                user.Password = request.Password;
            }

            PersonService.Update(user.Person);
            UserService.Update(user);

            var viewModel = new UserViewModel(user);

            if (request.Email != null && !request.Email.Equals(user.Login))
            {
                var temp = UserTempService.GetAll()
                    .FirstOrDefault(x => x.User == user);

                if (temp == null)
                {
                    temp = new UserTemp { User = user };
                    UserTempService.Create(temp);
                }

                temp.Email = request.Email;
                temp.TempCode = RandomNumber();
                UserTempService.Update(temp);

                await EmailService.SendChangeMessage(temp.Email, temp.TempCode);

                viewModel.Person.Email = temp.Email;
            }

            return Json(new DataResponse<UserViewModel>
            {
                Data = viewModel
            });
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToUser([FromBody] SubscribeRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subject = UserService.Get(request.SubjectID);
            if (subject == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Subject not found"
                });

            var subscription = UserSubscriptionService.GetAll()
                .FirstOrDefault(x => x.Subscriber == user && x.Subject == subject);

            if (subscription == null)
            {
                subscription = new UserSubscription
                {
                    Subject = subject,
                    Subscriber = user,
                    IsActive = true
                };

                UserSubscriptionService.Create(subscription);
            }

            subscription.IsActive = true;
            UserSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        [HttpDelete("subscribe")]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscribeRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subject = UserService.Get(request.SubjectID);
            if (subject == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Subject not found"
                });

            var subscription = UserSubscriptionService.GetAll()
                .FirstOrDefault(x => x.Subscriber == user && x.Subject == subject);

            if (subscription == null)
            {
                subscription = new UserSubscription
                {
                    Subject = subject,
                    Subscriber = user,
                    IsActive = false
                };

                UserSubscriptionService.Create(subscription);
            }

            subscription.IsActive = false;
            UserSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        private ProfileViewModel GetProfileModel(User user)
        {
            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .Select(x => new VehicleViewModel(x))
                .ToList();

            var profile = new ProfileViewModel
            {
                FirstName = user.Person.FirstName,
                LastName = user.Person.LastName,
                Nickname = user.Nickname,
                PointsCount = user.PointsCount,
                CityName = user.Person.City.Name,
                SubscribersCount = 0,
                Biography = user.Person.Biography,
                Rewards = new List<RewardViewModel>(),
                Vehicles = vehicles
            };

            if (user.Group != null)
                profile.GroupName = user.Group.Name;

            if (user.Avatar != null)
                profile.AvatarUrl = user.Avatar.Url;

            return profile;
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}