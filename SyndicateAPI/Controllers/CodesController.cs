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
    [Route("api/codes")]
    [ApiController]
    [Authorize]
    public class CodesController : Controller
    {
        private IUserService UserService { get; set; }
        private IPointsCodeService PointsCodeService { get; set; }

        public CodesController([FromServices]
            IUserService userService,
            IPointsCodeService pointsCodeService)
        {
            UserService = userService;
            PointsCodeService = pointsCodeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCode([FromBody] CreatePointsCodeRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var now = DateTime.UtcNow;
            var code = RandomNumber().ToString();

            var pointsCode = PointsCodeService.GetAll()
                .FirstOrDefault(x => x.Code == code);

            if (pointsCode == null)
            {
                pointsCode = new PointsCode
                {
                    Code = code,
                    PointsCount = request.PointsCount,
                    CreationDate = now,
                    ExpiresDate = now.AddMinutes(10),
                    IsUsed = false,
                    Creator = user
                };

                PointsCodeService.Create(pointsCode);
            }
            else
            {
                pointsCode.ExpiresDate = now.AddMinutes(10);
                pointsCode.IsUsed = false;
                PointsCodeService.Update(pointsCode);
            }

            return Ok(new DataResponse<PointsCodeViewModel>
            {
                Data = new PointsCodeViewModel(pointsCode)
            });
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitCode([FromBody] SubmitPointsCodeRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var code = PointsCodeService.GetAll()
                .FirstOrDefault(x => x.Code == request.Code);

            if (code == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Код не действителен"
                });

            if (code.IsUsed)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Код не действителен"
                });

            if (code.Creator == user)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Код не действителен"
                });

            if (code.ExpiresDate < DateTime.UtcNow)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Код не действителен"
                });

            code.IsUsed = true;
            PointsCodeService.Update(code);

            user.PointsCount += code.PointsCount;
            UserService.Update(user);

            return Ok(new SubmitPointsCodeResponse
            {
                PointsAwarded = code.PointsCount
            });
        }

        private long RandomNumber()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
    }
}