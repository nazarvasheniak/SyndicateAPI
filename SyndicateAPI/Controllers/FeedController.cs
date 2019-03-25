using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private IRatingLevelService RatingLevelService { get; set; }

        public FeedController([FromBody]
            IUserService userService,
            IPostService postService,
            IFileService fileService,
            IRatingLevelService ratingLevelService)
        {
            UserService = userService;
            PostService = postService;
            FileService = fileService;
            RatingLevelService = ratingLevelService;
        }

        [HttpPost("post/user")]
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
                .FirstOrDefault(x => x.Login == User.Identity.Name);

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

        [HttpPost("post/group")]
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
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user.Group == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not in group"
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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post)
            });
        }
    }
}