using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/ratingLevels")]
    [ApiController]
    public class RatingLevelsController : Controller
    {
        private IUserService UserService { get; set; }
        private IRatingLevelService RatingLevelService { get; set; }

        public RatingLevelsController([FromServices]
            IUserService userService,
            IRatingLevelService ratingLevelService)
        {
            UserService = userService;
            RatingLevelService = ratingLevelService;
        }

        [HttpPost("createStartData")]
        public async Task<IActionResult> CreateStartData()
        {
            var existLevels = RatingLevelService.GetAll();
            foreach (var level in existLevels)
                RatingLevelService.Delete(level);

            var levels = new List<RatingLevel>()
            {
                new RatingLevel { PointsCount = 0, Title = "Новичок" },
                new RatingLevel { PointsCount = 101, Title = "Приезжий" },
                new RatingLevel { PointsCount = 301, Title = "Гонщик" },
                new RatingLevel { PointsCount = 501, Title = "Профессионал" }
            };

            foreach (var level in levels)
                RatingLevelService.Create(level);

            return Ok(new ResponseModel());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRatingLevels()
        {
            var ratingLevels = RatingLevelService.GetAll()
                .Select(x => new RatingLevelViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<RatingLevelViewModel>>
            {
                Data = ratingLevels
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRatingLevel(long id)
        {
            var ratingLevel = RatingLevelService.Get(id);
            if (ratingLevel == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level not found"
                });

            return Ok(new DataResponse<RatingLevelViewModel>
            {
                Data = new RatingLevelViewModel(ratingLevel)
            });
        }

        [HttpGet("{pointsCount}")]
        [Authorize]
        public async Task<IActionResult> GetRatingLevel(int pointsCount)
        {
            var ratingLevel = RatingLevelService.GetAll()
                .FirstOrDefault(x => x.PointsCount == pointsCount);

            if (ratingLevel == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level not found"
                });

            return Ok(new DataResponse<RatingLevelViewModel>
            {
                Data = new RatingLevelViewModel(ratingLevel)
            });
        }

        [HttpGet("{name}")]
        [Authorize]
        public async Task<IActionResult> GetRatingLevel(string name)
        {
            var ratingLevel = RatingLevelService.GetAll()
                .FirstOrDefault(x => x.Title == name);

            if (ratingLevel == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Rating level not found"
                });

            return Ok(new DataResponse<RatingLevelViewModel>
            {
                Data = new RatingLevelViewModel(ratingLevel)
            });
        }
    }
}