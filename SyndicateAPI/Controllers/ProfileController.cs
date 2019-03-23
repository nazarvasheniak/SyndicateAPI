﻿using System;
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
    public class ProfileController : Controller
    {
        private IUserService UserService { get; set; }
        private IPersonService PersonService { get; set; }
        private IRewardService RewardService { get; set; }
        private ICityService CityService { get; set; }
        private IVehicleService VehicleService { get; set; }

        public ProfileController([FromServices]
            IUserService userService,
            IPersonService personService,
            IRewardService rewardService,
            ICityService cityService,
            IVehicleService vehicleService)
        {
            UserService = userService;
            PersonService = personService;
            RewardService = rewardService;
            CityService = cityService;
            VehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var user = UserService.GetAll().FirstOrDefault(x => x.Login == User.Identity.Name);
            var profile = GetProfileModel(user);
            
            return Ok(new DataResponse<ProfileViewModel>
            {
                Data = profile
            });
        }

        [HttpGet("vehicles")]
        public async Task<IActionResult> GetVehicles()
        {
            var user = UserService.GetAll().FirstOrDefault(x => x.Login == User.Identity.Name);
            var vehicles = VehicleService.GetAll().Where(x => x.Owner == user).Select(x => new VehicleViewModel(x)).ToList();

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

            var vehicles = VehicleService.GetAll().Where(x => x.Owner == user).Select(x => new VehicleViewModel(x)).ToList();

            return Ok(new DataResponse<List<VehicleViewModel>>
            {
                Data = vehicles
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
            if (!request.Password.Equals(request.ConfirmPassword))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Passwords are not equals"
                });

            var user = UserService.GetAll().FirstOrDefault(x => x.Login == User.Identity.Name);

            if (!request.FirstName.Equals(user.Person.FirstName))
                user.Person.FirstName = request.FirstName;

            if (!request.LastName.Equals(user.Person.LastName))
                user.Person.LastName = request.LastName;

            if (!request.Nickname.Equals(user.Nickname))
                user.Nickname = request.Nickname;

            if (!request.Email.Equals(user.Login))
            {
                user.Login = request.Email;
                user.Person.Email = request.Email;
            }

            if (!request.CityID.Equals(user.Person.City.ID))
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

            if (!request.Biography.Equals(user.Person.Biography))
                user.Person.Biography = request.Biography;

            if (!request.Password.Equals(user.Password))
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

            return Ok(new DataResponse<UserViewModel>
            {
                Data = new UserViewModel(user)
            });
        }

        private ProfileViewModel GetProfileModel(User user)
        {
            var profile = new ProfileViewModel
            {
                FirstName = user.Person.FirstName,
                LastName = user.Person.LastName,
                Nickname = user.Nickname,
                PointsCount = user.PointsCount,
                CityName = user.Person.City.Name,
                SubscribersCount = 0,
                Biography = user.Person.Biography,
                Rewards = new List<RewardViewModel>()
            };

            if (user.Group != null)
                profile.GroupName = user.Group.Name;

            if (user.Avatar != null)
                profile.AvatarUrl = user.Avatar.Url;

            return profile;
        }
    }
}