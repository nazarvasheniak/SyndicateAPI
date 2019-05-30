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
using SyndicateAPI.WebSocketManager;

namespace SyndicateAPI.Controllers
{
    [Route("api/feed")]
    [ApiController]
    [Authorize]
    public class FeedController : Controller
    {
        private IUserService UserService { get; set; }
        private IFileService FileService { get; set; }
        private IPostService PostService { get; set; }
        private IPostLikeService PostLikeService { get; set; }
        private IPostCommentService PostCommentService { get; set; }
        private IPostCommentLikeService PostCommentLikeService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IGroupMemberService GroupMemberService { get; set; }
        private IGroupCreatorService GroupCreatorService { get; set; }
        private IGroupModeratorService GroupModeratorService { get; set; }
        private IGroupSubscriptionService GroupSubscriptionService { get; set; }
        private IGroupJoinRequestService GroupJoinRequestService { get; set; }
        private IRatingLevelService RatingLevelService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }
        private NotificationsMessageHandler Notifications { get; set; }

        public FeedController([FromBody]
            IUserService userService,
            IFileService fileService,
            IPostService postService,
            IPostLikeService postLikeService,
            IPostCommentService postCommentService,
            IPostCommentLikeService postCommentLikeService,
            IGroupPostService groupPostService,
            IGroupMemberService groupMemberService,
            IGroupCreatorService groupCreatorService,
            IGroupModeratorService groupModeratorService,
            IGroupSubscriptionService groupSubscriptionService,
            IGroupJoinRequestService groupJoinRequestService,
            IRatingLevelService ratingLevelService,
            IUserSubscriptionService userSubscriptionService,
            NotificationsMessageHandler notifications)
        {
            UserService = userService;
            FileService = fileService;
            PostService = postService;
            PostLikeService = postLikeService;
            PostCommentService = postCommentService;
            PostCommentLikeService = postCommentLikeService;
            GroupPostService = groupPostService;
            GroupMemberService = groupMemberService;
            GroupCreatorService = groupCreatorService;
            GroupModeratorService = groupModeratorService;
            GroupSubscriptionService = groupSubscriptionService;
            GroupJoinRequestService = groupJoinRequestService;
            RatingLevelService = ratingLevelService;
            UserSubscriptionService = userSubscriptionService;
            Notifications = notifications;
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

        [HttpGet("user")]
        public async Task<IActionResult> GetUserFeed([FromQuery] GetListRequest request)
        {
            var req = Request;
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subsriptions = UserSubscriptionService.GetAll()
                .Where(x => x.Subscriber == user && x.IsActive)
                .ToList();

            var feed = new List<PostViewModel>();

            foreach (var subscription in subsriptions)
            {
                var posts = PostToViewModel(PostService.GetAll()
                    .Where(x => 
                        x.Author == subscription.Subject &&
                        x.Type == PostType.User &&
                        x.IsPublished));

                if (posts != null && posts.Count != 0)
                    feed.AddRange(posts);
            }

            var myPosts = PostToViewModel(PostService.GetAll()
                .Where(x => 
                    x.Author == user &&
                    x.Type == PostType.User &&
                    x.IsPublished));

            feed.AddRange(myPosts);

            if (feed.Count != 0)
                feed = feed.OrderByDescending(x => x.PublishTime)
                    .Skip((request.PageNumber - 1) * request.PageCount)
                    .Take(request.PageCount)
                    .ToList();

            return Ok(new DataResponse<List<PostViewModel>>
            {
                Data = feed
            });
        }

        [HttpGet("group")]
        public async Task<IActionResult> GetGroupFeed([FromQuery] GetListRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            var userGroupSubscriptions = GroupSubscriptionService.GetAll()
                .Where(x => x.User == user && x.IsActive)
                .ToList();

            var feed = new List<GroupPostViewModel>();
            var posts = new List<GroupPost>();

            if (userGroupMember != null)
            {
                var groupMemberPosts = GroupPostService.GetAll()
                    .Where(x => x.Group == userGroupMember.Group)
                    .ToList();

                foreach (var p in groupMemberPosts)
                    posts.Add(p);
            }

            foreach (var s in userGroupSubscriptions)
            {
                var ps = GroupPostService.GetAll()
                    .Where(x => x.Group == s.Group)
                    .ToList();

                foreach (var p in ps)
                    posts.Add(p);
            }

            posts = new List<GroupPost>(new HashSet<GroupPost>(posts));

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

            foreach (var post in posts)
                feed.Add(new GroupPostViewModel(post, subscribers, members, role, joinRequests));

            if (feed != null && feed.Count != 0)
                feed = feed.OrderByDescending(x => x.Post.PublishTime)
                    .Skip((request.PageNumber - 1) * request.PageCount)
                    .Take(request.PageCount)
                    .ToList();

            feed = feed
                .Select(x =>
                {
                    x.Post = PostToViewModel(x.Post);
                    return x;
                })
                .ToList();

            return Ok(new DataResponse<List<GroupPostViewModel>>
            {
                Data = feed
            });
        }

        [HttpPost("user")]
        public async Task<IActionResult> PublishUserPost([FromBody] PublishPostRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var image = FileService.Get(request.ImageID);
            if (image == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Image error"
                });

            DateTime publishTime;

            if (request.PublishTime != null && request.PublishTime != DateTime.MinValue)
            {
                publishTime = TimeZoneInfo.ConvertTimeToUtc(request.PublishTime.ToUniversalTime());
                if (request.PublishTime.IsDaylightSavingTime())
                    publishTime = publishTime.Subtract(TimeSpan.FromHours(1));
            }
            else
            {
                publishTime = DateTime.UtcNow;
            }

            var post = new Post
            {
                Text = request.Text,
                Type = PostType.User,
                PublishTime = publishTime,
                Author = user,
                RatingScore = request.RatingScore,
                Image = image,
                Latitude = request.Latitude,
                Longitude = request.Longtitude
            };

            if (post.PublishTime <= DateTime.UtcNow)
                post.IsPublished = true;
            else
                post.IsPublished = false;

            PostService.Create(post);

            var subscribers = UserSubscriptionService.GetAll()
                .Where(x => x.Subject == user && x.IsActive)
                .Select(x => x.Subscriber)
                .ToList();

            foreach (var subscriber in subscribers)
                await Notifications
                    .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, 
                    new { postID = post.ID });

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post)
            });
        }

        [HttpPost("group")]
        public async Task<IActionResult> PublishGroupPost([FromBody] PublishPostRequest request)
        {
            var image = FileService.Get(request.ImageID);
            if (image == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Image error"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (userGroupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == userGroupMember.Group && x.IsActive);

            if (user != userGroupMember.Group.Owner && userGroupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете публиковать посты в данной группировке"
                });

            if (userGroupModerator != null && userGroupModerator.Level == GroupModeratorLevel.Level1)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете публиковать посты в данной группировке"
                });

            DateTime publishTime;

            if (request.PublishTime != null && request.PublishTime != DateTime.MinValue)
            {
                publishTime = TimeZoneInfo.ConvertTimeToUtc(request.PublishTime.ToUniversalTime());
                if (request.PublishTime.IsDaylightSavingTime())
                    publishTime = publishTime.Subtract(TimeSpan.FromHours(1));
            }
            else
            {
                publishTime = DateTime.UtcNow;
            }

            var post = new Post
            {
                Text = request.Text,
                Type = PostType.Group,
                PublishTime = publishTime,
                Author = user,
                RatingScore = request.RatingScore,
                Image = image,
                Latitude = request.Latitude,
                Longitude = request.Longtitude
            };

            if (request.PublishTime.ToUniversalTime() <= DateTime.UtcNow)
                post.IsPublished = true;
            else
                post.IsPublished = false;

            PostService.Create(post);

            var groupPost = new GroupPost
            {
                Post = post,
                Group = userGroupMember.Group
            };

            GroupPostService.Create(groupPost);

            var groupMembers = GroupMemberService.GetAll()
                .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            var groupSubscribers = GroupSubscriptionService.GetAll()
                .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                .Select(x => new UserViewModel(x.User))
                .ToList();

            foreach (var member in groupMembers)
                await Notifications
                    .SendUpdateToUser(member.ID,SocketMessageType.GroupPostUpdate,
                    new { groupPostID = groupPost.ID });

            foreach (var subscriber in groupSubscribers)
                await Notifications
                    .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate,
                    new { groupPostID = groupPost.ID });

            return Ok(new DataResponse<GroupPostViewModel>
            {
                Data = new GroupPostViewModel(groupPost)
            });
        }

        [HttpPut("{postID}")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request, long postID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var userGroupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (userGroupMember == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не состоите в группировке"
                });

            var userGroupModerator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Group == userGroupMember.Group && x.IsActive);

            if (user != userGroupMember.Group.Owner && userGroupModerator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете редактировать посты в данной группировке"
                });

            if (userGroupModerator != null && userGroupModerator.Level == GroupModeratorLevel.Level1)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете редактировать посты в данной группировке"
                });

            if (request.Text != null && request.Text != post.Text)
                post.Text = request.Text;

            if (request.RatingScore != post.RatingScore)
                post.RatingScore = request.RatingScore;

            DateTime publishTime;

            if (request.PublishTime != null && request.PublishTime != DateTime.MinValue)
            {
                publishTime = TimeZoneInfo.ConvertTimeToUtc(request.PublishTime.ToUniversalTime());
                if (request.PublishTime.IsDaylightSavingTime())
                    publishTime = publishTime.Subtract(TimeSpan.FromHours(1));
            }
            else
            {
                publishTime = DateTime.UtcNow;
            }

            if (request.Latitude != post.Latitude || request.Longtitude != post.Longitude)
            {
                post.Latitude = request.Latitude;
                post.Longitude = request.Longtitude;
            }

            PostService.Update(post);

            var likes = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            bool isLiked;
            if (likes.FirstOrDefault(x => x.User == user) == null)
                isLiked = false;
            else
                isLiked = true;

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

            var result = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpDelete("{postID}")]
        public async Task<IActionResult> DeletePost(long postID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            if (post.Type == PostType.Group)
            {
                var userGroupMember = GroupMemberService.GetAll()
                    .FirstOrDefault(x => x.User == user && x.IsActive);

                if (userGroupMember == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Вы не состоите в группировке"
                    });

                var userGroupModerator = GroupModeratorService.GetAll()
                    .FirstOrDefault(x => x.User == user && x.Group == userGroupMember.Group && x.IsActive);

                if (user != userGroupMember.Group.Owner && userGroupModerator == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Вы не можете удалять посты в данной группировке"
                    });

                if (userGroupModerator != null && userGroupModerator.Level == GroupModeratorLevel.Level1)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Вы не можете удалять посты в данной группировке"
                    });

                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                GroupPostService.Delete(groupPost);

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == userGroupMember.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, 
                        new { groupPostID = groupPost.ID});

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate,
                        new { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate,
                        new { postID = post.ID });
            }

            PostService.Delete(post);

            return Ok(new ResponseModel());
        }

        [HttpPost("{postID}/like")]
        public async Task<IActionResult> LikePost(long postID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var like = PostLikeService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Post == post);

            if (like == null)
            {
                like = new PostLike
                {
                    Post = post,
                    User = user
                };

                PostLikeService.Create(like);
            }

            var likesCount = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList()
                .Count;

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

            var result = new PostViewModel(post, viewComments, true, (ulong)likesCount);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpDelete("{postID}/like")]
        public async Task<IActionResult> UnlikePost(long postID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var like = PostLikeService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Post == post);

            if (like != null)
                PostLikeService.Delete(like);

            var likesCount = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList()
                .Count;

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

            var result = new PostViewModel(post, viewComments, false, (ulong)likesCount);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpPost("{postID}/comment")]
        public async Task<IActionResult> PublishComment([FromBody] PublishCommentRequest request, long postID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var comment = new PostComment
            {
                Text = request.Text,
                Time = DateTime.UtcNow,
                Post = post,
                Author = user
            };

            PostCommentService.Create(comment);

            var likes = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            bool isLiked;

            if (likes.FirstOrDefault(x => x.User == user) == null)
                isLiked = false;
            else
                isLiked = true;

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

            var result = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpDelete("{postID}/comment/{commentID}")]
        public async Task<IActionResult> DeleteComment(long postID, long commentID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var comment = PostCommentService.Get(commentID);
            if (comment == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Comment ID error"
                });

            var moderator = GroupModeratorService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            if (comment.Author != user && moderator == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "You are not author"
                });

            PostCommentService.Delete(commentID);

            var likes = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            bool isLiked;

            if (likes.FirstOrDefault(x => x.User == user) == null)
                isLiked = false;
            else
                isLiked = true;

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

            var result = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpPost("{postID}/comment/{commentID}/like")]
        public async Task<IActionResult> LikePostComment(long postID, long commentID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var comment = PostCommentService.Get(commentID);
            if (comment == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Comment ID error"
                });

            var like = PostCommentLikeService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Comment == comment);

            if (like == null)
            {
                like = new PostCommentLike
                {
                    Comment = comment,
                    User = user
                };

                PostCommentLikeService.Create(like);
            }

            var myPostLike = false;
            if (PostLikeService.GetAll()
                .FirstOrDefault(x => x.Post == post && x.User == user) != null)
            {
                myPostLike = true;
            }

            var likesCount = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList()
                .Count;

            var comments = PostCommentService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            var viewComments = new List<PostCommentViewModel>();

            foreach (var c in comments)
            {
                var isLiked = false;
                var likes = PostCommentLikeService.GetAll()
                    .Where(x => x.Comment == c)
                    .ToList();

                var myLike = PostCommentLikeService.GetAll()
                    .FirstOrDefault(x => x.Comment == c && x.User == user);

                if (myLike != null)
                    isLiked = true;

                viewComments.Add(new PostCommentViewModel(c, isLiked, (ulong)likes.Count));
            }

            var result = new PostViewModel(post, viewComments, myPostLike, (ulong)likesCount);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }

        [HttpDelete("{postID}/comment/{commentID}/like")]
        public async Task<IActionResult> UnlikePostComment(long postID, long commentID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            var comment = PostCommentService.Get(commentID);
            if (comment == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Comment ID error"
                });

            var like = PostCommentLikeService.GetAll()
                .FirstOrDefault(x => x.User == user && x.Comment == comment);

            if (like != null)
                PostCommentLikeService.Delete(like);

            var myPostLike = false;
            if (PostLikeService.GetAll()
                .FirstOrDefault(x => x.Post == post && x.User == user) != null)
            {
                myPostLike = true;
            }

            var likesCount = PostLikeService.GetAll()
                .Where(x => x.Post == post)
                .ToList()
                .Count;

            var comments = PostCommentService.GetAll()
                .Where(x => x.Post == post)
                .ToList();

            var viewComments = new List<PostCommentViewModel>();

            foreach (var c in comments)
            {
                var isLiked = false;
                var likes = PostCommentLikeService.GetAll()
                    .Where(x => x.Comment == c)
                    .ToList();

                var myLike = PostCommentLikeService.GetAll()
                    .FirstOrDefault(x => x.Comment == c && x.User == user);

                if (myLike != null)
                    isLiked = true;

                viewComments.Add(new PostCommentViewModel(c, isLiked, (ulong)likes.Count));
            }

            var result = new PostViewModel(post, viewComments, myPostLike, (ulong)likesCount);

            if (post.Type == PostType.Group)
            {
                var groupPost = GroupPostService.GetAll()
                    .FirstOrDefault(x => x.Post == post);

                if (groupPost == null)
                    return BadRequest(new ResponseModel
                    {
                        Success = false,
                        Message = "Group post error"
                    });

                var groupMembers = GroupMemberService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                var groupSubscribers = GroupSubscriptionService.GetAll()
                    .Where(x => x.Group == groupPost.Group && x.IsActive)
                    .Select(x => new UserViewModel(x.User))
                    .ToList();

                foreach (var member in groupMembers)
                    await Notifications
                        .SendUpdateToUser(member.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });

                foreach (var subscriber in groupSubscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.GroupPostUpdate, new
                        { groupPostID = groupPost.ID });
            }
            else
            {
                var subscribers = UserSubscriptionService.GetAll()
                    .Where(x => x.Subject == user && x.IsActive)
                    .Select(x => new UserViewModel(x.Subscriber))
                    .ToList();

                foreach (var subscriber in subscribers)
                    await Notifications
                        .SendUpdateToUser(subscriber.ID, SocketMessageType.PostUpdate, new
                        { postID = post.ID });
            }

            return Ok(new DataResponse<PostViewModel>
            {
                Data = result
            });
        }
    }
}