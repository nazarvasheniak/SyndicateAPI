using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/admin/ratingLevels")]
    [ApiController]
    [Authorize]
    public class RatingLevelsController : Controller
    {
        private IAdminUserService AdminUserService { get; set; }
        private IRatingLevelService RatingLevelService { get; set; }

        public RatingLevelsController([FromServices]
            IAdminUserService adminUserService,
            IRatingLevelService ratingLevelService)
        {
            AdminUserService = adminUserService;
            RatingLevelService = ratingLevelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var ratingLevels = RatingLevelService.GetAll()
                .Select(x => new RatingLevelViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<RatingLevelViewModel>>
            {
                Data = ratingLevels

            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingLevel(long id)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var ratingLevel = RatingLevelService.Get(id);
            if (ratingLevel == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level not found"
                });

            return Ok(new DataResponse<RatingLevelViewModel>
            {
                Data = new RatingLevelViewModel(ratingLevel)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRatingLevel([FromBody] CreateRatingLevelRequest request)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var ratingLevel = RatingLevelService.GetAll()
                .FirstOrDefault(x => x.Title.ToLower() == request.Title.ToLower());

            if (ratingLevel != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = $"Rating level with title '{request.Title}' already exist"
                });

            ratingLevel = RatingLevelService.GetAll()
                .FirstOrDefault(x => x.PointsCount == request.PointsCount);

            if (ratingLevel != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = $"Rating level with points count '{request.PointsCount}' already exist"
                });

            ratingLevel = new RatingLevel
            {
                Title = request.Title,
                PointsCount = request.PointsCount
            };

            RatingLevelService.Create(ratingLevel);

            return Ok(new DataResponse<RatingLevelViewModel>
            {
                Data = new RatingLevelViewModel(ratingLevel)
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRatingLevel(long id)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var ratingLevel = RatingLevelService.Get(id);
            if (ratingLevel == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level not found"
                });

            RatingLevelService.Delete(ratingLevel.ID);

            return Ok(new ResponseModel());
        }
    }
}