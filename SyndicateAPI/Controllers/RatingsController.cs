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
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    [Authorize]
    public class RatingsController : Controller
    {
        private IUserService UserService { get; set; }
        private IGroupService GroupService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehiclePhotoService VehiclePhotoService { get; set; }

        public RatingsController([FromServices]
            IUserService userService,
            IGroupService groupService,
            IGroupPostService groupPostService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService)
        {
            UserService = userService;
            GroupService = groupService;
            GroupPostService = groupPostService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserRatings([FromQuery] string type)
        {
            var users = UserService.GetAll()
                    .OrderByDescending(x => x.PointsCount);

            if (type.Equals("user"))
                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = users.Select(x => new UserViewModel(x)).ToList()
                });
            else if (type.Equals("profile"))
                return Ok(new DataResponse<List<ProfileViewModel>>
                {
                    Data = users.Select(x => GetProfileModel(x)).ToList()
                });
            else
                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = users.Select(x => new UserViewModel(x)).ToList()
                });
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroupRatings()
        {
            var ratingList = new Dictionary<GroupViewModel, long>();

            var groups = GroupService.GetAll().ToList();
            foreach (var group in groups)
            {
                var groupUsers = UserService.GetAll()
                    .Where(x => x.Group == group)
                    .ToList();

                if (groupUsers == null || groupUsers.Count == 0)
                    ratingList.Add(new GroupViewModel(group), 0);
                else
                    ratingList.Add(new GroupViewModel(group), groupUsers.Sum(x => x.PointsCount));
            }

            var result = new List<GroupViewModel>();
            foreach (var group in ratingList.OrderByDescending(x => x.Value).ToList())
                result.Add(group.Key);

            return Ok(new DataResponse<List<GroupViewModel>>
            {
                Data = result
            });
        }

        private ProfileViewModel GetProfileModel(User user)
        {
            var vehicles = VehicleService.GetAll()
                .Where(x => x.Owner == user)
                .ToList();

            var viewVehicles = new List<VehicleViewModel>();

            foreach (var vehicle in vehicles)
            {
                var photos = VehiclePhotoService.GetAll()
                    .Where(x => x.Vehicle == vehicle)
                    .ToList();

                viewVehicles.Add(new VehicleViewModel(vehicle, photos));
            }

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
                Vehicles = viewVehicles
            };

            if (user.Group != null)
                profile.GroupName = user.Group.Name;

            if (user.Avatar != null)
                profile.AvatarUrl = user.Avatar.Url;

            return profile;
        }
    }
}