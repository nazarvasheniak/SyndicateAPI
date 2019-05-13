using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Enums;
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
        private IGroupMemberService GroupMemberService { get; set; }
        private IGroupModeratorService GroupModeratorService { get; set; }
        private IGroupCreatorService GroupCreatorService { get; set; }
        private IGroupSubscriptionService GroupSubscriptionService { get; set; }
        private IGroupJoinRequestService GroupJoinRequestService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehiclePhotoService VehiclePhotoService { get; set; }
        private IAwardService AwardService { get; set; }

        public RatingsController([FromServices]
            IUserService userService,
            IGroupService groupService,
            IGroupPostService groupPostService,
            IGroupMemberService groupMemberService,
            IGroupModeratorService groupModeratorService,
            IGroupCreatorService groupCreatorService,
            IGroupSubscriptionService groupSubscriptionService,
            IGroupJoinRequestService groupJoinRequestService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService,
            IAwardService awardService)
        {
            UserService = userService;
            GroupService = groupService;
            GroupPostService = groupPostService;
            GroupMemberService = groupMemberService;
            GroupModeratorService = groupModeratorService;
            GroupCreatorService = groupCreatorService;
            GroupSubscriptionService = groupSubscriptionService;
            GroupJoinRequestService = groupJoinRequestService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
            AwardService = awardService;
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

        private GroupMemberViewModel MemberToViewModel(GroupMember member)
        {
            var result = new GroupMemberViewModel(member);

            var creator = GroupCreatorService.GetAll()
                .FirstOrDefault(x =>
                    x.User == member.User &&
                    x.Group == member.Group &&
                    x.IsActive);

            if (creator != null)
            {
                result.Role = RoleInGroup.Creator;
                return result;
            }

            var moderator = GroupModeratorService.GetAll()
                .FirstOrDefault(x =>
                    x.User == member.User &&
                    x.Group == member.Group &&
                    x.IsActive);

            if (moderator != null)
            {
                switch (moderator.Level)
                {
                    case GroupModeratorLevel.Level1:
                        result.Role = RoleInGroup.ModeratorLevel1;
                        break;
                    case GroupModeratorLevel.Level2:
                        result.Role = RoleInGroup.ModeratorLevel2;
                        break;
                    case GroupModeratorLevel.Level3:
                        result.Role = RoleInGroup.ModeratorLevel3;
                        break;
                    default:
                        result.Role = RoleInGroup.Member;
                        break;
                }

                return result;
            }

            return result;
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroupRatings()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var ratingList = new Dictionary<GroupViewModel, long>();

            var groups = GroupService.GetAll().ToList();
            foreach (var group in groups)
            {
                var posts = GroupPostService.GetAll()
                    .Where(x => x.Group == group && x.Post.IsPublished)
                    .Select(x => new GroupPostViewModel(x))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == group && x.IsActive)
                    .Select(x => MemberToViewModel(x))
                    .ToList();

                RoleInGroup role;
                if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == group && x.IsActive) != null)
                    role = RoleInGroup.Creator;
                else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == group && x.IsActive) != null)
                {
                    var moder = GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                        x.Group == group && x.IsActive);

                    switch (moder.Level)
                    {
                        case GroupModeratorLevel.Level1:
                            role = RoleInGroup.ModeratorLevel1;
                            break;
                        case GroupModeratorLevel.Level2:
                            role = RoleInGroup.ModeratorLevel2;
                            break;
                        case GroupModeratorLevel.Level3:
                            role = RoleInGroup.ModeratorLevel3;
                            break;
                        default:
                            role = RoleInGroup.Member;
                            break;
                    }
                }
                else
                    role = RoleInGroup.Member;

                var joinRequests = GroupJoinRequestService.GetAll()
                    .Where(x => x.Group == group && x.Status == GroupJoinRequestStatus.New)
                    .Select(x => new GroupJoinRequestViewModel(x))
                    .ToList();

                if (groupMembers.Count == 0)
                    ratingList.Add(new GroupViewModel(group, posts, groupSubscribers, groupMembers, role, joinRequests), 0);
                else
                    ratingList.Add(new GroupViewModel(group, posts, groupSubscribers, groupMembers, role, joinRequests), groupMembers.Sum(x => x.User.PointsCount));
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

            var awards = AwardService.GetAll()
                .Where(x => x.Rewarder == user)
                .Select(x => new AwardViewModel(x))
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
                Awards = awards,
                Vehicles = viewVehicles
            };

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember != null)
            {
                var posts = GroupPostService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.Post.IsPublished)
                    .Select(x => new GroupPostViewModel(x))
                    .ToList();

                RoleInGroup role;
                if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == groupMember.Group) != null)
                    role = RoleInGroup.Creator;
                else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == groupMember.Group) != null)
                {
                    var moder = GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                        x.Group == groupMember.Group);

                    switch (moder.Level)
                    {
                        case GroupModeratorLevel.Level1:
                            role = RoleInGroup.ModeratorLevel1;
                            break;
                        case GroupModeratorLevel.Level2:
                            role = RoleInGroup.ModeratorLevel2;
                            break;
                        case GroupModeratorLevel.Level3:
                            role = RoleInGroup.ModeratorLevel3;
                            break;
                        default:
                            role = RoleInGroup.Member;
                            break;
                    }
                }
                else
                    role = RoleInGroup.Member;

                var subscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var members = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.IsActive)
                    .Select(x => MemberToViewModel(x))
                    .ToList();

                var joinRequests = GroupJoinRequestService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.Status == GroupJoinRequestStatus.New)
                    .Select(x => new GroupJoinRequestViewModel(x))
                    .ToList();

                profile.GroupName = groupMember.Group.Name;
                profile.Group = new GroupViewModel(groupMember.Group, posts, subscribers, members, role, joinRequests);
            }

            if (user.Avatar != null)
                profile.AvatarUrl = user.Avatar.Url;

            return profile;
        }
    }
}