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
    public class RewardsController : Controller
    {
        private IUserService UserService { get; set; }
        private IRewardService RewardService { get; set; }
        private IAwardService AwardService { get; set; }
        private IFileService FileService { get; set; }

        public RewardsController([FromServices]
            IUserService userService,
            IRewardService rewardService,
            IAwardService awardService,
            IFileService fileService)
        {
            UserService = userService;
            RewardService = rewardService;
            AwardService = awardService;
            FileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRewards()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var rewards = RewardService.GetAll()
                .Where(x => x.User == user)
                .Select(x => new RewardViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<RewardViewModel>>
            {
                Data = rewards
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRewardByID(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var reward = RewardService.Get(id);
            if (reward == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            if (reward.User != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Доступ ограничен"
                });

            return Ok(new DataResponse<RewardViewModel>
            {
                Data = new RewardViewModel(reward)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReward([FromBody] CreateRewardRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var icon = FileService.Get(request.IconID);
            if (icon == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Иконка не найдена"
                });

            var rewards = RewardService.GetAll()
                .Where(x => x.User == user)
                .ToList();

            var reward = new Reward
            {
                Name = $"Награда #{rewards.Count + 1}",
                Icon = icon,
                User = user
            };

            RewardService.Create(reward);

            return Ok(new DataResponse<RewardViewModel>
            {
                Data = new RewardViewModel(reward)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReward([FromBody] UpdateRewardRequest request, long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var reward = RewardService.Get(id);
            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            if (reward.User != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Доступ ограничен"
                });


            if (request.IconID != 0)
            {
                var icon = FileService.Get(request.IconID);
                if (icon == null)
                    return NotFound(new ResponseModel
                    {
                        Success = false,
                        Message = "Иконка не найдена"
                    });

                reward.Icon = icon;
            }

            RewardService.Update(reward);

            return Ok(new DataResponse<RewardViewModel>
            {
                Data = new RewardViewModel(reward)
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReward(long id)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var reward = RewardService.Get(id);
            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            if (reward.User != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Доступ ограничен"
                });

            RewardService.Delete(reward);

            return Ok(new ResponseModel());
        }

        [HttpPost("award")]
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

            if (reward.User != user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не пренадлежит вам"
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