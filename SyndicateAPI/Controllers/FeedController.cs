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
    [Route("api/feed")]
    [ApiController]
    [Authorize]
    public class FeedController : Controller
    {
        private IUserService UserService { get; set; }
        private IPostService PostService { get; set; }
        private IFileService FileService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IRatingLevelService RatingLevelService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }

        public FeedController([FromBody]
            IUserService userService,
            IPostService postService,
            IFileService fileService,
            IGroupPostService groupPostService,
            IRatingLevelService ratingLevelService,
            IUserSubscriptionService userSubscriptionService)
        {
            UserService = userService;
            PostService = postService;
            FileService = fileService;
            GroupPostService = groupPostService;
            RatingLevelService = ratingLevelService;
            UserSubscriptionService = userSubscriptionService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserFeed()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subsriptions = UserSubscriptionService.GetAll()
                .Where(x => x.Subscriber == user)
                .ToList();

            if (subsriptions == null || subsriptions.Count == 0)
                return Ok(new ResponseModel
                {
                    Message = "Вы не подписаны на людей"
                });

            var feed = new List<PostViewModel>();
            
            foreach (var subscription in subsriptions)
            {
                var posts = PostService.GetAll()
                    .Where(x => x.Author == subscription.Subject && x.IsPublished && x.Type == PostType.User)
                    .Select(x => new PostViewModel(x))
                    .ToList();

                if (posts != null && posts.Count != 0)
                    feed.AddRange(posts);
            }

            if (feed.Count != 0)
                feed = feed.OrderBy(x => x.PublishTime).ToList();

            return Ok(new DataResponse<List<PostViewModel>>
            {
                Data = feed
            });
        }

        [HttpGet("group")]
        public async Task<IActionResult> GetGroupFeed()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.Group == null)
                return Ok(new ResponseModel
                {
                    Message = "Вы не состоите в группировке"
                });

            var feed = GroupPostService.GetAll()
                .Where(x => x.Group == user.Group)
                .Select(x => new GroupPostViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<GroupPostViewModel>>
            {
                Data = feed
            });
        }

        [HttpPost("user")]
        public async Task<IActionResult> PublishUserPost([FromBody] PublishPostRequest request)
        {
            var ratingLevel = RatingLevelService.Get(request.RatingLevelID);
            if (ratingLevel == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level error"
                });

            var image = FileService.Get(request.ImageID);
            if (image == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Image error"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var post = new Post
            {
                Text = request.Text,
                Type = PostType.User,
                PublishTime = request.PublishTime,
                Author = user,
                MinRatingLevel = ratingLevel,
                Image = image
            };

            if (request.PublishTime.ToUniversalTime() > DateTime.UtcNow)
                post.IsPublished = false;
            else
                post.IsPublished = true;

            PostService.Create(post);

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post)
            });
        }

        [HttpPost("group")]
        public async Task<IActionResult> PublishGroupPost([FromBody] PublishPostRequest request)
        {
            var ratingLevel = RatingLevelService.Get(request.RatingLevelID);
            if (ratingLevel == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level error"
                });

            var image = FileService.Get(request.ImageID);
            if (image == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Image error"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (user.Group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not in group"
                });

            if (user != user.Group.Owner)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы не можете публиковать посты в данной группировке"
                });
            
            var post = new Post
            {
                Text = request.Text,
                Type = PostType.Group,
                PublishTime = request.PublishTime,
                Author = user,
                MinRatingLevel = ratingLevel,
                Image = image
            };

            if (request.PublishTime.ToUniversalTime() > DateTime.UtcNow)
                post.IsPublished = false;
            else
                post.IsPublished = true;

            PostService.Create(post);

            var groupPost = new GroupPost
            {
                Post = post,
                Group = user.Group
            };

            GroupPostService.Create(groupPost);

            return Ok(new DataResponse<GroupPostViewModel>
            {
                Data = new GroupPostViewModel(groupPost)
            });
        }
    }
}