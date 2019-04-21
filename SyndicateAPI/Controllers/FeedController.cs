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
        private IFileService FileService { get; set; }
        private IPostService PostService { get; set; }
        private IPostLikeService PostLikeService { get; set; }
        private IPostCommentService PostCommentService { get; set; }
        private IPostCommentLikeService PostCommentLikeService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IRatingLevelService RatingLevelService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }

        public FeedController([FromBody]
            IUserService userService,
            IFileService fileService,
            IPostService postService,
            IPostLikeService postLikeService,
            IPostCommentService postCommentService,
            IPostCommentLikeService postCommentLikeService,
            IGroupPostService groupPostService,
            IRatingLevelService ratingLevelService,
            IUserSubscriptionService userSubscriptionService)
        {
            UserService = userService;
            FileService = fileService;
            PostService = postService;
            PostLikeService = postLikeService;
            PostCommentService = postCommentService;
            PostCommentLikeService = postCommentLikeService;
            GroupPostService = groupPostService;
            RatingLevelService = ratingLevelService;
            UserSubscriptionService = userSubscriptionService;
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

        [HttpGet("user")]
        public async Task<IActionResult> GetUserFeed([FromQuery] GetPostsRequest request)
        {
            var req = Request;
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var subsriptions = UserSubscriptionService.GetAll()
                .Where(x => x.Subscriber == user)
                .ToList();

            var feed = new List<PostViewModel>();

            foreach (var subscription in subsriptions)
            {
                var posts = PostToViewModel(PostService.GetAll()
                    .Where(x => x.Author == subscription.Subject && x.IsPublished && x.Type == PostType.User));

                if (posts != null && posts.Count != 0)
                    feed.AddRange(posts);
            }

            var myPosts = PostToViewModel(PostService.GetAll()
                .Where(x => x.Author == user && x.IsPublished));

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
        public async Task<IActionResult> GetGroupFeed([FromQuery] GetPostsRequest request)
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

            if (feed != null && feed.Count != 0)
                feed = feed.OrderByDescending(x => x.Post.PublishTime)
                    .Skip((request.PageNumber - 1) * request.PageCount)
                    .Take(request.PageCount)
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
                Group = user.Group
            };

            GroupPostService.Create(groupPost);

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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count)
            });
        }

        [HttpDelete("{postID}")]
        public async Task<IActionResult> DeletePost(long postID)
        {
            var post = PostService.Get(postID);
            if (post == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Post ID error"
                });

            PostService.Delete(postID);

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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count)
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

            if (comment.Author != user)
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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post, viewComments, isLiked, (ulong)likes.Count)
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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post, viewComments, myPostLike, (ulong)likesCount)
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

            return Ok(new DataResponse<PostViewModel>
            {
                Data = new PostViewModel(post, viewComments, myPostLike, (ulong)likesCount)
            });
        }
    }
}