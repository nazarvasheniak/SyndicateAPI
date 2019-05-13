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

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/admin/rewards")]
    [ApiController]
    [Authorize]
    public class RewardsController : Controller
    {
        private IAdminUserService AdminUserService { get; set; }
        private IRewardService RewardService { get; set; }
        private IFileService FileService { get; set; }

        public RewardsController([FromServices]
            IAdminUserService adminUserService,
            IRewardService rewardService,
            IFileService fileService)
        {
            AdminUserService = adminUserService;
            RewardService = rewardService;
            FileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRewards()
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var rewards = RewardService.GetAll()
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
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.Get(id);
            if (reward == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Reward not found"
                });

            return Ok(new DataResponse<RewardViewModel>
            {
                Data = new RewardViewModel(reward)
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetRewardByName(string name)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.GetAll()
                .FirstOrDefault(x => x.Name == name);

            if (reward == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Reward not found"
                });

            return Ok(new DataResponse<RewardViewModel>
            {
                Data = new RewardViewModel(reward)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReward([FromBody] CreateRewardRequest request)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.GetAll()
                .FirstOrDefault(x => x.Name == request.Name);

            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда с таким именем уже существует"
                });

            var icon = FileService.Get(request.IconID);
            if (icon == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Иконка не найдена"
                });

            reward = new Reward
            {
                Name = request.Name,
                Description = request.Description,
                Icon = icon
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
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.Get(id);
            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            if (request.Name != null && request.Name != reward.Name)
                reward.Name = request.Name;

            if (request.Description != null && request.Description != reward.Description)
                reward.Description = request.Description;

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

        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateReward([FromBody] UpdateRewardRequest request, string name)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.GetAll()
                .FirstOrDefault(x => x.Name == name);

            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            if (request.Name != null && request.Name != reward.Name)
                reward.Name = request.Name;

            if (request.Description != null && request.Description != reward.Description)
                reward.Description = request.Description;

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
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.Get(id);
            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            RewardService.Delete(reward);

            return Ok(new ResponseModel());
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteReward(string name)
        {
            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == User.Identity.Name);

            if (user == null)
                return Unauthorized(new ResponseModel
                {
                    Success = false,
                    Message = "Unauthorized"
                });

            var reward = RewardService.GetAll()
                .FirstOrDefault(x => x.Name == name);

            if (reward != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Награда не найдена"
                });

            RewardService.Delete(reward);

            return Ok(new ResponseModel());
        }
    }
}