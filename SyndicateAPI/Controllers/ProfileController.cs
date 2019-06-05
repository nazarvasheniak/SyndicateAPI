using System;
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
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : Controller
    {
        private IUserService UserService { get; set; }
        private IUserTempService UserTempService { get; set; }
        private IFileService FileService { get; set; }
        private IEmailService EmailService { get; set; }
        private IPersonService PersonService { get; set; }
        private IRewardService RewardService { get; set; }
        private IAwardService AwardService { get; set; }
        private ICityService CityService { get; set; }
        private IVehicleService VehicleService { get; set; }
        private IVehiclePhotoService VehiclePhotoService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }
        private IGroupMemberService GroupMemberService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IGroupSubscriptionService GroupSubscriptionService { get; set; }
        private IGroupModeratorService GroupModeratorService { get; set; }
        private IGroupCreatorService GroupCreatorService { get; set; }
        private IGroupJoinRequestService GroupJoinRequestService { get; set; }
        private IPostService PostService { get; set; }
        private IPostLikeService PostLikeService { get; set; }
        private IPostCommentService PostCommentService { get; set; }
        private IPostCommentLikeService PostCommentLikeService { get; set; }
        private IStartRewardService StartRewardService { get; set; }

        public ProfileController([FromServices]
            IUserService userService,
            IUserTempService userTempService,
            IFileService fileService,
            IEmailService emailService,
            IPersonService personService,
            IRewardService rewardService,
            IAwardService awardService,
            ICityService cityService,
            IVehicleService vehicleService,
            IVehiclePhotoService vehiclePhotoService,
            IUserSubscriptionService userSubscriptionService,
            IGroupMemberService groupMemberService,
            IGroupPostService groupPostService,
            IGroupSubscriptionService groupSubscriptionService,
            IGroupModeratorService groupModeratorService,
            IGroupCreatorService groupCreatorService,
            IGroupJoinRequestService groupJoinRequestService,
            IPostService postService,
            IPostLikeService postLikeService,
            IPostCommentService postCommentService,
            IPostCommentLikeService postCommentLikeService,
            IStartRewardService startRewardService)
        {
            UserService = userService;
            UserTempService = userTempService;
            FileService = fileService;
            EmailService = emailService;
            PersonService = personService;
            RewardService = rewardService;
            AwardService = awardService;
            CityService = cityService;
            VehicleService = vehicleService;
            VehiclePhotoService = vehiclePhotoService;
            UserSubscriptionService = userSubscriptionService;
            GroupMemberService = groupMemberService;
            GroupPostService = groupPostService;
            GroupSubscriptionService = groupSubscriptionService;
            GroupModeratorService = groupModeratorService;
            GroupCreatorService = groupCreatorService;
            GroupJoinRequestService = groupJoinRequestService;
            PostService = postService;
            PostLikeService = postLikeService;
            PostCommentService = postCommentService;
            PostCommentLikeService = postCommentLikeService;
            StartRewardService = startRewardService;
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

            var result = new UpdateProfileResponse();

            if (request.FirstName != null && !request.FirstName.Equals(user.Person.FirstName))
                user.Person.FirstName = request.FirstName;

            if (request.LastName != null && !request.LastName.Equals(user.Person.LastName))
                user.Person.LastName = request.LastName;

            if (request.Nickname != null && !request.Nickname.Equals(user.Nickname))
                user.Nickname = request.Nickname;

            if (request.CityID != 0 && request.CityID != user.Person.City.ID)
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

            if (request.AvatarID != 0)
            {
                if (user.Avatar == null || request.AvatarID != user.Avatar.ID)
                {
                    var avatar = FileService.Get(request.AvatarID);
                    if (avatar == null)
                        return BadRequest(new ResponseModel
                        {
                            Success = false,
                            Message = "Avatar error"
                        });

                    user.Avatar = avatar;

                    var startReward = StartRewardService.GetAll().FirstOrDefault(x => x.User == user);
                    if (!startReward.IsAvatarCompleted)
                    {
                        user.PointsCount += 30;
                        result.BonusPoints += 30;
                        result.Message = $"Вам начислено {result.BonusPoints} за обновление аватарки";

                        startReward.IsAvatarCompleted = true;
                        StartRewardService.Update(startReward);
                    }
                }
            }

            if (request.Biography != null && !request.Biography.Equals(user.Person.Biography))
            {
                user.Person.Biography = request.Biography;

                var startReward = StartRewardService.GetAll().FirstOrDefault(x => x.User == user);
                if (!startReward.IsBiographyCompleted)
                {
                    user.PointsCount += 30;
                    result.BonusPoints += 30;

                    if (result.Message.Equals("OK"))
                        result.Message = $"Вам начислено {result.BonusPoints} за обновление информации о себе";
                    else
                        result.Message += $" и обновление информации о себе";

                    startReward.IsBiographyCompleted = true;
                    StartRewardService.Update(startReward);
                }
            }

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

            result.Data = viewModel;

            return Json(result);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToUser([FromBody] SubscribeRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (request.SubjectID.Equals(user.ID))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Same ID error"
                });

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

        [HttpPost("{id}/subscribe")]
        public async Task<IActionResult> SubscribeToUser(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (id.Equals(user.ID))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Same ID error"
                });

            var subject = UserService.Get(id);
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

        private PostViewModel PostToViewModel(Post post)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var likes = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            var comments = PostCommentService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            var viewComments = new List<PostCommentViewModel>();

            foreach (var c in comments)
            {
                var isLikedComment = false;
                var commentLikes = PostCommentLikeService.GetAll()
                    .Where(x => x.Comment == c)
                    .ToList();

                var myLike = PostCommentLikeService.GetAll()
                    .FirstOrDefault(x => x.Comment == c && x.User == user);

                if (myLike != null)
                    isLikedComment = true;

                viewComments.Add(new PostCommentViewModel(c, isLikedComment, (ulong)commentLikes.Count));
            }

            var result = new PostViewModel(post)
            {
                Comments = viewComments,
                LikesCount = (ulong)likes.Count
            };

            if (likes.FirstOrDefault(x => x.User == user) == null)
                result.IsLiked = false;
            else
                result.IsLiked = true;

            return result;
        }

        private PostViewModel PostToViewModel(PostViewModel post)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var dbPost = PostService.Get(post.ID);
            if (dbPost == null)
                return null;

            var likes = PostLikeService.GetAll()
                .Where(x => x.Post == dbPost)
                .ToList();

            var comments = PostCommentService.GetAll()
                .Where(x => x.Post == dbPost)
                .ToList();

            var viewComments = new List<PostCommentViewModel>();

            foreach (var c in comments)
            {
                var isLikedComment = false;
                var commentLikes = PostCommentLikeService.GetAll()
                    .Where(x => x.Comment == c)
                    .ToList();

                var myLike = PostCommentLikeService.GetAll()
                    .FirstOrDefault(x => x.Comment == c && x.User == user);

                if (myLike != null)
                    isLikedComment = true;

                viewComments.Add(new PostCommentViewModel(c, isLikedComment, (ulong)commentLikes.Count));
            }

            var result = new PostViewModel(dbPost)
            {
                Comments = viewComments,
                LikesCount = (ulong)likes.Count
            };

            if (likes.FirstOrDefault(x => x.User == user) == null)
                result.IsLiked = false;
            else
                result.IsLiked = true;

            return result;
        }

        private List<PostViewModel> PostToViewModel(IEnumerable<Post> posts)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var result = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var likes = PostLikeService.GetAll()
                    .Where(x => x.Post == post)
                    .ToList();

                var comments = PostCommentService.GetAll()
                    .Where(x => x.Post == post)
                    .ToList();

                var viewComments = new List<PostCommentViewModel>();

                foreach (var c in comments)
                {
                    var isLikedComment = false;
                    var commentLikes = PostCommentLikeService.GetAll()
                        .Where(x => x.Comment == c)
                        .ToList();

                    var myLike = PostCommentLikeService.GetAll()
                        .FirstOrDefault(x => x.Comment == c && x.User == user);

                    if (myLike != null)
                        isLikedComment = true;

                    viewComments.Add(new PostCommentViewModel(c, isLikedComment, (ulong)commentLikes.Count));
                }

                var postItem = new PostViewModel(post)
                {
                    Comments = viewComments,
                    LikesCount = (ulong)likes.Count
                };

                if (likes.FirstOrDefault(x => x.User == user) == null)
                    postItem.IsLiked = false;
                else
                    postItem.IsLiked = true;

                result.Add(postItem);
            }

            return result;
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
                return Ok(new ResponseModel());

            subscription.IsActive = false;
            UserSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        [HttpDelete("{id}/subscribe")]
        public async Task<IActionResult> Unsubscribe(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subject = UserService.Get(id);
            if (subject == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Subject not found"
                });

            var subscription = UserSubscriptionService.GetAll()
                .FirstOrDefault(x => x.Subscriber == user && x.Subject == subject);

            if (subscription == null)
                return Ok(new ResponseModel());

            subscription.IsActive = false;
            UserSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        [HttpGet("subscribers")]
        public async Task<IActionResult> GetSubscribers([FromQuery] string type)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (type.Equals("user"))
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscribers
                });
            }
            else if (type.Equals("profile"))
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => GetProfileModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<ProfileViewModel>>
                {
                    Data = subscribers
                });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscribers
                });
            }
        }

        [HttpGet("{id}/subscribers")]
        public async Task<IActionResult> GetSubscribers([FromQuery] string type, long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID == id);

            if (user == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            if (type.Equals("user"))
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscribers
                });
            }
            else if (type.Equals("profile"))
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => GetProfileModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<ProfileViewModel>>
                {
                    Data = subscribers
                });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .OrderByDescending(x => x.Subscriber.PointsCount)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscribers
                });
            }
        }

        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetSubscriptions([FromQuery] string type)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (type.Equals("user"))
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => new UserViewModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscriptions
                });
            }
            else if (type.Equals("profile"))
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => GetProfileModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<ProfileViewModel>>
                {
                    Data = subscriptions
                });
            }
            else
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => new UserViewModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscriptions
                });
            }
        }

        [HttpGet("{id}/subscriptions")]
        public async Task<IActionResult> GetSubscriptions([FromQuery] string type, long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID == id);

            if (user == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            if (type.Equals("user"))
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => new UserViewModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscriptions
                });
            }
            else if (type.Equals("profile"))
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => GetProfileModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<ProfileViewModel>>
                {
                    Data = subscriptions
                });
            }
            else
            {
                var subscriptions = UserSubscriptionService.GetAll()
                    .Where(x => x.Subscriber == user && x.IsActive)
                    .OrderByDescending(x => x.Subject.PointsCount)
                    .Select(x => new UserViewModel(x.Subject))
                    .ToList();

                return Ok(new DataResponse<List<UserViewModel>>
                {
                    Data = subscriptions
                });
            }
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
                Biography = user.Person.Biography,
                Awards = awards,
                Vehicles = viewVehicles,
                IsOnline = user.IsOnline,
                Coordinates = new CoordinatesViewModel(user.Latitude, user.Longitude)
            };

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember != null)
            {
                var posts = GroupPostService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.Post.IsPublished)
                    .Select(x => new GroupPostViewModel(x))
                    .ToList();

                posts = posts.Select(x =>
                {
                    x.Post = PostToViewModel(x.Post);
                    return x;
                })
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
                    .OrderByDescending(x => x.User.PointsCount)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var members = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.IsActive)
                    .OrderByDescending(x => x.User.PointsCount)
                    .Select(x => MemberToViewModel(x))
                    .ToList();

                var joinRequests = GroupJoinRequestService.GetAll()
                    .Where(x => x.Group == groupMember.Group && x.Status == GroupJoinRequestStatus.New)
                    .OrderByDescending(x => x.User.PointsCount)
                    .Select(x => new GroupJoinRequestViewModel(x))
                    .ToList();

                profile.GroupName = groupMember.Group.Name;
                profile.Group = new GroupViewModel(groupMember.Group, posts, subscribers, members, role, joinRequests);
            }

            if (user.Avatar != null)
                profile.AvatarUrl = user.Avatar.Url;

            profile.Subscribers = UserSubscriptionService.GetAll()
                .Where(x => x.Subject == user && x.IsActive)
                .OrderByDescending(x => x.Subscriber.PointsCount)
                .Select(x => new UserViewModel(x.Subscriber))
                .ToList();

            profile.SubscribersCount = profile.Subscribers.Count();

            profile.Subscriptions = UserSubscriptionService.GetAll()
                .Where(x => x.Subscriber == user && x.IsActive)
                .OrderByDescending(x => x.Subject.PointsCount)
                .Select(x => new UserViewModel(x.Subject))
                .ToList();

            return profile;
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}