using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
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

        public MapController([FromServices]
            IUserService userService,
            IMapPointService mapPointService,
            IPartnerService partnerService)
        {
            UserService = userService;
            MapPointService = mapPointService;
            PartnerService = partnerService;
        }

        [HttpGet("points")]
        public async Task<IActionResult> GetMapPoints()
        {
            var points = MapPointService.GetAll()
                .Select(x => new MapPointViewModel(x))
                .ToList();

            var partners = PartnerService.GetAll()
                .Select(x => new PartnerViewModel(x))
                .ToList();

            return Ok(new MapPointsResponse
            {
                Points = points,
                Partners = partners
            });
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
                MapPointType = request.MapPointType
            };

            MapPointService.Create(point);

            return Ok(new DataResponse<MapPointViewModel>
            {
                Data = new MapPointViewModel(point)
            });
        }
    }
}