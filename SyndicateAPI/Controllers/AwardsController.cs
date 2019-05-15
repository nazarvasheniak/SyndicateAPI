using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/rewards")]
    [ApiController]
    [Authorize]
    public class AwardsController : Controller
    {
        private IUserService UserService { get; set; }
        private IRewardService RewardService { get; set; }
        private IAwardService AwardService { get; set; }

        public AwardsController([FromServices]
            IUserService userService,
            IRewardService rewardService,
            IAwardService awardService)
        {
            UserService = userService;
            RewardService = rewardService;
            AwardService = awardService;
        }

        [HttpGet("rewards")]
        public async Task<IActionResult> GetAllRewards()
        {
            var rewards = RewardService.GetAll()
                .Select(x => new RewardViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<RewardViewModel>>
            {
                Data = rewards
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAward([FromBody] CreateAwardRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var rewarder = UserService.Get(request.RewarderID);
            if (rewarder == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Награждаемый пользователь не найден"
                });

            var reward = RewardService.Get(request.RewardID);
            if (reward == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            var award = new Award
            {
                Awarder = user,
                Rewarder = rewarder,
                Comment = request.Comment
            };

            AwardService.Create(award);

            return Ok(new DataResponse<AwardViewModel>
            {
                Data = new AwardViewModel(award)
            });
        }
    }
}