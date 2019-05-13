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
        private IPostLikeService PostLikeService { get; set; }
        private IPostCommentService PostCommentService { get; set; }
        private IPostCommentLikeService PostCommentLikeService { get; set; }
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
            IPostLikeService postLikeService,
            IPostCommentService postCommentService,
            IPostCommentLikeService postCommentLikeService,
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
            PostLikeService = postLikeService;
            PostCommentService = postCommentService;
            PostCommentLikeService = postCommentLikeService;
            GroupPostService = groupPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x =>
                    x.User == user &&
                    x.IsActive);

            if (userGroupMember == null)
                return Ok(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var posts = GroupPostService.GetAll()
                .Where(x => x.Group == userGroupMember.Group && x.Post.IsPublished)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            RoleInGroup role;
            if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == userGroupMember.Group) != null)
                role = RoleInGroup.Creator;
            else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == userGroupMember.Group) != null)
            {
                var moder = GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == userGroupMember.Group);

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
                .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => MemberToViewModel(x))
                .ToList();

            var joinRequests = GroupJoinRequestService.GetAll()
                .Where(x => x.Group == userGroupMember.Group && x.Status == GroupJoinRequestStatus.New)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new GroupJoinRequestViewModel(x))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(userGroupMember.Group, posts, subscribers, members, role, joinRequests)
            });
        }

        [HttpGet("{groupID}")]
        public async Task<IActionResult> GetGroup(long groupID)
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

            var posts = GroupPostService.GetAll()
                .Where(x => x.Group == group && x.Post.IsPublished)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            RoleInGroup role;
            if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == group) != null)
                role = RoleInGroup.Creator;
            else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == group) != null)
            {
                var moder = GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == group);

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
                role = 0;

            var subscribers = GroupSubscriptionService.GetAll()
                .Where(x => x.Group == group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => MemberToViewModel(x))
                .ToList();

            var joinRequests = GroupJoinRequestService.GetAll()
                .Where(x => x.Group == group && x.Status == GroupJoinRequestStatus.New)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new GroupJoinRequestViewModel(x))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group, posts, subscribers, members, role, joinRequests)
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

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == group && x.IsActive)
                .Select(x => MemberToViewModel(x))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group, new List<GroupPostViewModel>(), new List<UserViewModel>(), members, RoleInGroup.Creator, new List<GroupJoinRequestViewModel>())
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
                .Where(x => x.Group == group && x.Post.IsPublished)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            RoleInGroup role;
            if (GroupCreatorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == group) != null)
                role = RoleInGroup.Creator;
            else if (GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                x.Group == group) != null)
            {
                var moder = GroupModeratorService.GetAll().FirstOrDefault(x => x.User == user &&
                    x.Group == group);

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
                .Where(x => x.Group == group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == group && x.IsActive)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => MemberToViewModel(x))
                .ToList();

            var joinRequests = GroupJoinRequestService.GetAll()
                .Where(x => x.Group == group && x.Status == GroupJoinRequestStatus.New)
                .OrderByDescending(x => x.User.PointsCount)
                .Select(x => new GroupJoinRequestViewModel(x))
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group, posts, subscribers, members, role, joinRequests)
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x =>
                    x.User == user &&
                    x.IsActive);

            if (userGroupMember == null)
                return Ok(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });
                
            if (userGroupMember.Group.Owner.ID != user.ID)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не создатель группировки"
                });

            var groupID = userGroupMember.Group.ID;

            var moderators = GroupModeratorService.GetAll()
                .Where(x => x.Group == userGroupMember.Group)
                .ToList();

            foreach (var moderator in moderators)
                GroupModeratorService.Delete(moderator);

            var creators = GroupCreatorService.GetAll()
                .Where(x => x.Group == userGroupMember.Group)
                .ToList();

            foreach (var creator in creators)
                GroupCreatorService.Delete(creator);

            var members = GroupMemberService.GetAll()
                .Where(x => x.Group == userGroupMember.Group)
                .ToList();

            foreach (var member in members)
                GroupMemberService.Delete(member);
            
            GroupService.Delete(groupID);

            return Ok(new ResponseModel());
        }

        [HttpPost("moderator")]
        public async Task<IActionResult> AddModerator([FromBody] AddUpdateModeratorRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var groupCreator = GroupCreatorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            var groupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            if (groupCreator == null && groupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            if (groupModerator != null && groupModerator.Level != GroupModeratorLevel.Level3)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            var requestUser = UserService.Get(request.UserID);
            if (requestUser == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x =>
                    x.User == requestUser &&
                    x.Group == groupMember.Group &&
                    x.IsActive);

            if (userGroupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не состоит в группировке"
                });

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x =>
                    x.User == requestUser &&
                    x.Group == userGroupMember.Group);

            if (userGroupModerator == null)
            {
                userGroupModerator = new GroupModerator
                {
                    User = userGroupMember.User,
                    Group = userGroupMember.Group,
                    Level = request.Level,
                    IsActive = true
                };

                GroupModeratorService.Create(userGroupModerator);
            }

            userGroupModerator.IsActive = true;
            GroupModeratorService.Update(userGroupModerator);

            return Ok(new ResponseModel());
        }

        [HttpPut("moderator")]
        public async Task<IActionResult> UpdateModerator([FromBody] AddUpdateModeratorRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var groupCreator = GroupCreatorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            var groupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            if (groupCreator == null && groupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            if (groupModerator != null && groupModerator.Level != GroupModeratorLevel.Level3)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            var requestUser = UserService.Get(request.UserID);
            if (requestUser == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x =>
                    x.User == requestUser &&
                    x.Group == groupMember.Group &&
                    x.IsActive);

            if (userGroupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не состоит в группировке"
                });

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x =>
                    x.User == requestUser &&
                    x.Group == userGroupMember.Group);

            if (userGroupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не является модератором"
                });

            userGroupModerator.Level = request.Level;
            GroupModeratorService.Update(userGroupModerator);

            return Ok(new ResponseModel());
        }

        [HttpDelete("moderator")]
        public async Task<IActionResult> RemoveModerator([FromBody] RemoveModeratorRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var groupCreator = GroupCreatorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            var groupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == groupMember.Group && x.IsActive);

            if (groupCreator == null && groupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            if (groupModerator != null && groupModerator.Level != GroupModeratorLevel.Level3)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Недостаточно прав"
                });

            var requestUser = UserService.Get(request.UserID);
            if (requestUser == null)
                return Ok(new ResponseModel());

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == requestUser && x.Group == groupMember.Group && x.IsActive);

            if (userGroupModerator == null)
                return Ok(new ResponseModel());

            userGroupModerator.IsActive = false;
            GroupModeratorService.Update(userGroupModerator);

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

            var request = GroupJoinRequestService.GetAll()
                .FirstOrDefault(x =>
                    x.User == user &&
                    x.Group == group);

            if (request == null)
            {
                request = new GroupJoinRequest
                {
                    User = user,
                    Group = group,
                    Status = GroupJoinRequestStatus.New
                };

                GroupJoinRequestService.Create(request);
            }

            request.Status = GroupJoinRequestStatus.New;
            GroupJoinRequestService.Update(request);

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

            GroupJoinRequestService.Delete(request);

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
                .FirstOrDefault(x => x.Group == group && x.User == user && x.IsActive);

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
            GroupJoinRequestService.Update(joinRequest);

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == joinRequest.User && x.Group == group);

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
                .FirstOrDefault(x => x.Group == group && x.User == user && x.IsActive);

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
            GroupJoinRequestService.Update(joinRequest);

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
                .FirstOrDefault(x => x.User == user && x.Group == group && x.IsActive);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в данной группировке"
                });

            groupMember.IsActive = false;
            GroupMemberService.Update(groupMember);

            var groupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == group && x.IsActive);

            if (groupModerator != null)
            {
                groupModerator.IsActive = false;
                GroupModeratorService.Update(groupModerator);
            }

            return Ok(new ResponseModel());
        }

        [HttpDelete("member/{memberID}")]
        public async Task<IActionResult> DeleteFromGroup(long memberID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (groupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Group not found"
                });

            var moderator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.Group == groupMember.Group && x.User == user && x.IsActive);

            if (moderator == null && groupMember.Group.Owner != user)
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

            var userGroupMember = GroupMemberService.Get(memberID);
            if (userGroupMember == null)
                return Ok(new ResponseModel());

            userGroupMember.IsActive = false;
            GroupMemberService.Update(userGroupMember);

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == userGroupMember.User && x.Group == userGroupMember.Group && x.IsActive);

            if (userGroupModerator != null)
            {
                userGroupModerator.IsActive = false;
                GroupModeratorService.Update(userGroupModerator);
            }

            return Ok(new ResponseModel());
        }
    }
}