using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/[controller]")]
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
    }
}