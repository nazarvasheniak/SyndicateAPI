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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : Controller
    {
        private IUserService UserService { get; set; }
        private IGroupService GroupService { get; set; }
        private IFileService FileService { get; set; }
        private IPostService PostService { get; set; }
        private IGroupPostService GroupPostService { get; set; }

        public GroupsController([FromServices]
            IUserService userService,
            IGroupService groupService,
            IFileService fileService,
            IPostService postService,
            IGroupPostService groupPostService)
        {
            UserService = userService;
            GroupService = groupService;
            FileService = fileService;
            PostService = postService;
            GroupPostService = groupPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user.Group == null)
                return Ok(new ResponseModel
                {
                    Message = "Вы не состоите в группировке"
                });

            var posts = GroupPostService.GetAll()
                .Where(x => x.Group == user.Group)
                .ToList();

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(user.Group, posts)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

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

            var avatar = FileService.Get(request.AvatarID);
            if (avatar == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Avatar not found"
                });

            group = new Group
            {
                Name = request.Name,
                ShortDesc = request.ShortDesc,
                FullDesc = request.FullDesc,
                Avatar = avatar,
                Owner = user
            };

            GroupService.Create(group);

            return Ok(new DataResponse<GroupViewModel>
            {
                Data = new GroupViewModel(group)
            });
        }
    }
}