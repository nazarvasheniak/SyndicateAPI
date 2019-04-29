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
    [Route("api/groups")]
    [ApiController]
    [Authorize]
    public class GroupsController : Controller
    {
        private IUserService UserService { get; set; }
        private IGroupService GroupService { get; set; }
        private IGroupSubscriptionService GroupSubscriptionService { get; set; }
        private IGroupJoinRequestService GroupJoinRequestService { get; set; }
        private IGroupModeratorService GroupModeratorService { get; set; }
        private IGroupCreatorService GroupCreatorService { get; set; }
        private IGroupMemberService GroupMemberService { get; set; }
        private IFileService FileService { get; set; }
        private IPostService PostService { get; set; }
        private IGroupPostService GroupPostService { get; set; }

        public GroupsController([FromServices]
            IUserService userService,
            IGroupService groupService,
            IGroupSubscriptionService groupSubscriptionService,
            IGroupJoinRequestService groupJoinRequestService,
            IGroupModeratorService groupModeratorService,
            IGroupCreatorService groupCreatorService,
            IGroupMemberService groupMemberService,
            IFileService fileService,
            IPostService postService,
            IGroupPostService groupPostService)
        {
            UserService = userService;
            GroupService = groupService;
            GroupSubscriptionService = groupSubscriptionService;
            GroupJoinRequestService = groupJoinRequestService;
            GroupModeratorService = groupModeratorService;
            GroupCreatorService = groupCreatorService;
            GroupMemberService = groupMemberService;
            FileService = fileService;
            PostService = postService;
            GroupPostService = groupPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.Group == null)
                return Ok(new ResponseModel
                {
                    Message = "Вы не состоите в группировке"
                });

            var posts = GroupPostService.GetAll()
                .Where(x => x.Group == user.Group && x.Post.IsPublished)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            RoleInGroup role;
            if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == user.Group) != null)
                role = RoleInGroup.Creator;
            else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == user.Group) != null)
                role = RoleInGroup.Moderator;
            else
                role = RoleInGroup.Member;

            var subscribers = GroupSubscriptionService.GetAll()
                .Where(x => x.Group == user.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == user.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(user.Group, posts, subscribers, members, role)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.GetAll()
                .FirstOrDefault(x => x.Name == request.Name);

            if (group != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Группировка с таким именем уже существует"
                });

            group = GroupService.GetAll()
                .FirstOrDefault(x => x.Owner == user);

            if (group != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы уже создали группировку"
                });

            group = new Group
            {
                Name = request.Name,
                ShortDesc = request.ShortDesc,
                FullDesc = request.FullDesc,
                Information = request.Information,
                Owner = user
            };

            GroupService.Create(group);

            GroupMemberService.Create(new GroupMember
            {
                Group = group,
                User = user,
                IsActive = true
            });

            GroupCreatorService.Create(new GroupCreator
            {
                User = user,
                Group = group,
                IsActive = true
            });

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group)
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.GetAll()
                .FirstOrDefault(x => x.Owner == user);

            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            if (request.Name != null && request.Name != group.Name)
                group.Name = request.Name;

            if (request.ShortDesc != null && request.ShortDesc != group.ShortDesc)
                group.ShortDesc = request.ShortDesc;

            if (request.FullDesc != null && request.FullDesc != group.FullDesc)
                group.FullDesc = request.FullDesc;

            if (request.Information != null && request.Information != group.Information)
                group.Information = request.Information;

            if (request.AvatarID != 0)
            {
                var file = FileService.Get(request.AvatarID);
                if (file == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "File error"
                    });

                group.Avatar = file;
            }

            GroupService.Update(group);

            var posts = GroupPostService.GetAll()
                .Where(x => x.Group == user.Group && x.Post.IsPublished)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            RoleInGroup role;
            if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == user.Group) != null)
                role = RoleInGroup.Creator;
            else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == user.Group) != null)
                role = RoleInGroup.Moderator;
            else
                role = RoleInGroup.Member;

            var subscribers = GroupSubscriptionService.GetAll()
                .Where(x => x.Group == user.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == user.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group, posts, subscribers, members, role)
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.Group == null)
                return Ok(new ResponseModel
                {
                    Message = "Вы не состоите в группировке"
                });

            if (user.Group.Owner.ID != user.ID)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не создатель группировки"
                });

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == user.Group)
                .ToList();

            foreach (var member in members)
                GroupMemberService.Delete(member);

            var moderators = GroupModeratorService.GetAll()
                .Where(x => x.Group == user.Group)
                .ToList();

            foreach (var moderator in moderators)
                GroupModeratorService.Delete(moderator);

            var creator = GroupCreatorService.GetAll()
                .FirstOrDefault(x => x.Group == user.Group);

            if (creator != null)
                GroupCreatorService.Delete(creator);

            return Ok(new ResponseModel());
        }

        [HttpPost("{groupID}/subscribe")]
        public async Task<IActionResult> SubscribeToGroup(long groupID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var subscription = GroupSubscriptionService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == group);

            if (subscription == null)
            {
                subscription = new GroupSubscription
                {
                    Group = group,
                    User = user,
                    IsActive = true
                };

                GroupSubscriptionService.Create(subscription);
            }

            subscription.IsActive = true;
            GroupSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        [HttpDelete("{groupID}/subscribe")]
        public async Task<IActionResult> UnsubscribeFromGroup(long groupID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var subscription = GroupSubscriptionService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == group);

            if (subscription == null)
            {
                subscription = new GroupSubscription
                {
                    Group = group,
                    User = user,
                    IsActive = false
                };

                GroupSubscriptionService.Create(subscription);
            }

            subscription.IsActive = false;
            GroupSubscriptionService.Update(subscription);

            return Ok(new ResponseModel());
        }

        [HttpPost("{groupID}/join")]
        public async Task<IActionResult> JoinToGroupRequest(long groupID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            if (group.Owner == user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы владелец группировки"
                });

            var request = new GroupJoinRequest
            {
                User = user,
                Group = group,
                Status = GroupJoinRequestStatus.New
            };

            GroupJoinRequestService.Create(request);

            return Ok(new ResponseModel());
        }

        [HttpDelete("{groupID}/join")]
        public async Task<IActionResult> DeleteJoinToGroup(long groupID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            if (group.Owner == user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы владелец группировки"
                });

            var request = GroupJoinRequestService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == group);

            if (request.Status != GroupJoinRequestStatus.New)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Невозможно отменить обработанную заявку"
                });

            GroupJoinRequestService.Delete
(request);

            return Ok(new ResponseModel());
        }

        [HttpPost("{groupID}/join/{requestID}/approve")]
        public async Task<IActionResult> ApproveJoinRequestStatus(long groupID, long requestID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var joinRequest = GroupJoinRequestService.Get(requestID);
            if (joinRequest == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Join request not found"
                });

            var moderator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.Group == group && x.User == user);

            if (moderator == null && group.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            if (moderator != null && moderator.Level != GroupModeratorLevel.Level3)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            joinRequest.Status = GroupJoinRequestStatus.Approved;

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == joinRequest.User);

            if (groupMember == null)
            {
                groupMember = new GroupMember
                {
                    User = joinRequest.User,
                    Group = group,
                    IsActive = true
                };

                GroupMemberService.Create(groupMember);
            }

            groupMember.IsActive = true;
            GroupMemberService.Update(groupMember);

            return Ok(new ResponseModel());
        }

        [HttpPost("{groupID}/join/{requestID}/reject")]
        public async Task<IActionResult> RejectJoinRequestStatus(long groupID, long requestID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var joinRequest = GroupJoinRequestService.Get(requestID);
            if (joinRequest == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Join request not found"
                });

            var moderator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.Group == group && x.User == user);

            if (moderator == null && group.Owner != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            if (moderator != null && moderator.Level != GroupModeratorLevel.Level3)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            joinRequest.Status = GroupJoinRequestStatus.Rejected;

            return Ok(new ResponseModel());
        }

        [HttpDelete("{groupID}/leave")]
        public async Task<IActionResult> LeaveGroup(long groupID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var group = GroupService.Get(groupID);
            if (group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в данной группировке"
                });

            groupMember.IsActive = false;
            GroupMemberService.Update(groupMember);

            return Ok(new ResponseModel());
        }

        //[HttpPost("{groupID}/members/{memberID}")]
    }
}