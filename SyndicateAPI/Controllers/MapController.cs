using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain;
using SyndicateAPI.Domain.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/map")]
    [ApiController]
    [Authorize]
    public class MapController : Controller
    {
        private IUserService UserService { get; set; }
        private IMapPointService MapPointService { get; set; }
        private IPartnerService PartnerService { get; set; }
        private IPartnerProductService PartnerProductService { get; set; }
        private IUserSubscriptionService UserSubscriptionService { get; set; }
        private IPostService PostService { get; set; }
        private IGroupPostService GroupPostService { get; set; }
        private IGroupMemberService GroupMemberService { get; set; }
        private IGroupSubscriptionService GroupSubscriptionService { get; set; }
        private IPostLikeService PostLikeService { get; set; }
        private IPostCommentService PostCommentService { get; set; }
        private IPostCommentLikeService PostCommentLikeService { get; set; }

        public MapController([FromServices]
            IUserService userService,
            IMapPointService mapPointService,
            IPartnerService partnerService,
            IPartnerProductService partnerProductService,
            IUserSubscriptionService userSubscriptionService,
            IPostService postService,
            IGroupPostService groupPostService,
            IGroupMemberService groupMemberService,
            IGroupSubscriptionService groupSubscriptionService,
            IPostLikeService postLikeService,
            IPostCommentService postCommentService,
            IPostCommentLikeService postCommentLikeService)
        {
            UserService = userService;
            MapPointService = mapPointService;
            PartnerService = partnerService;
            PartnerProductService = partnerProductService;
            UserSubscriptionService = userSubscriptionService;
            PostService = postService;
            GroupPostService = groupPostService;
            GroupMemberService = groupMemberService;
            GroupSubscriptionService = groupSubscriptionService;
            PostLikeService = postLikeService;
            PostCommentService = postCommentService;
            PostCommentLikeService = postCommentLikeService;
        }

        private PartnerViewModel PartnerToViewModel(Partner partner)
        {
            var products = PartnerProductService.GetAll()
                .Where(x => x.Partner == partner)
                .ToList();

            return new PartnerViewModel(partner, products);
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

        private GroupPostViewModel GroupPostToViewModel(GroupPost groupPost)
        {
            var result = new GroupPostViewModel(groupPost)
            {
                Post = PostToViewModel(groupPost.Post)
            };

            return result;
        }

        private bool IsMapObject(IMapPointObject item)
        {
            if (item.Latitude == 0 || item.Longitude == 0)
                return false;

            return true;
        }

        private bool IsInRadius(IMapPointObject item, double centerLatitude, double centerLongitude, double radius)
        {
            if (!IsMapObject(item))
                return false;

            var center = new GeoCoordinate(centerLatitude, centerLongitude);
            var coordinate = new GeoCoordinate(item.Latitude, item.Longitude);
            var distance = center.GetDistanceTo(coordinate);

            if (distance <= radius)
                return true;

            return false;
        }

        [HttpGet("points")]
        public async Task<IActionResult> GetMapPoints([FromQuery] GetMapPointsRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var result = new MapPointsResponse
            {
                Points = new List<MapPointViewModel>(),
                Partners = new List<PartnerViewModel>(),
                Users = new List<UserViewModel>(),
                Posts = new List<PostViewModel>(),
                GroupPosts = new List<GroupPostViewModel>()
            };

            var users = UserService.GetAll().Where(x => x.ID != user.ID).ToList();
            foreach (var u in users)
                if (IsInRadius(u, request.CenterLatitude, request.CenterLongitude, request.Radius))
                    result.Users.Add(new UserViewModel(u));

            var points = MapPointService.GetAll().ToList();
            foreach (var point in points)
                if (IsInRadius(point, request.CenterLatitude, request.CenterLongitude, request.Radius))
                    result.Points.Add(new MapPointViewModel(point));

            var partners = PartnerService.GetAll().ToList();
            foreach (var partner in partners)
                if (IsInRadius(partner, request.CenterLatitude, request.CenterLongitude, request.Radius))
                    result.Partners.Add(PartnerToViewModel(partner));

            var subscriptions = UserSubscriptionService.GetAll()
                .Where(x => x.Subscriber == user && x.IsActive)
                .ToList();

            var groupSubscriptions = GroupSubscriptionService.GetAll()
                .Where(x => x.User == user && x.IsActive)
                .ToList();

            var groupMember = GroupMemberService.GetAll()
                .FirstOrDefault(x => x.User == user && x.IsActive);

            foreach (var subscription in subscriptions)
            {
                var posts = PostService.GetAll()
                    .Where(x =>
                        x.Author == subscription.Subject &&
                        x.IsPublished)
                    .ToList();

                foreach (var post in posts)
                    if (IsInRadius(post, request.CenterLatitude, request.CenterLongitude, request.Radius))
                        result.Posts.Add(PostToViewModel(post));
            }

            foreach (var subscription in groupSubscriptions)
            {
                var posts = GroupPostService.GetAll()
                    .Where(x =>
                        x.Group == subscription.Group &&
                        x.Post.IsPublished)
                    .ToList();

                foreach (var post in posts)
                    if (IsInRadius(post.Post, request.CenterLatitude, request.CenterLongitude, request.Radius))
                        result.GroupPosts.Add(GroupPostToViewModel(post));
            }

            if (groupMember != null)
            {
                var posts = GroupPostService.GetAll()
                    .Where(x =>
                        x.Group == groupMember.Group &&
                        x.Post.IsPublished)
                    .ToList();

                foreach (var post in posts)
                    if (IsInRadius(post.Post, request.CenterLatitude, request.CenterLongitude, request.Radius))
                        result.GroupPosts.Add(GroupPostToViewModel(post));
            }

            return Ok(result);
        }

        [HttpPost("points")]
        public async Task<IActionResult> CreateMapPoint([FromBody] CreateMapPointRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var point = new MapPoint
            {
                Name = request.Name,
                Message = request.Message,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                MapPointType = request.MapPointType,
                User = user
            };

            MapPointService.Create(point);

            return Ok(new DataResponse<MapPointViewModel>
            {
                Data = new MapPointViewModel(point)
            });
        }

        [HttpDelete("points/{id}")]
        public async Task<IActionResult> DeleteMapPoint(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var point = MapPointService.Get(id);
            if (point == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Метка не найдена"
                });

            MapPointService.Delete(id);

            return Ok(new ResponseModel());
        }
    }
}