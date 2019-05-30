using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
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
        private IPartnerProductService PartnerProductService { get; set; }

        public MapController([FromServices]
            IUserService userService,
            IMapPointService mapPointService,
            IPartnerService partnerService,
            IPartnerProductService partnerProductService)
        {
            UserService = userService;
            MapPointService = mapPointService;
            PartnerService = partnerService;
            PartnerProductService = partnerProductService;
        }

        private PartnerViewModel PartnerToViewModel(Partner partner)
        {
            var products = PartnerProductService.GetAll()
                .Where(x => x.Partner == partner)
                .ToList();

            return new PartnerViewModel(partner, products);
        }

        [HttpGet("points")]
        public async Task<IActionResult> GetMapPoints([FromQuery] GetMapPointsRequest request)
        {
            var center = new GeoCoordinate(request.CenterLatitude, request.CenterLongitude);

            var result = new MapPointsResponse
            {
                Points = new List<MapPointViewModel>(),
                Partners = new List<PartnerViewModel>()
            };

            var points = MapPointService.GetAll().ToList();
            foreach (var point in points)
            {
                if (point.Latitude == 0 || point.Longitude == 0)
                    continue;

                var coordinate = new GeoCoordinate(point.Latitude, point.Longitude);
                var distance = center.GetDistanceTo(coordinate);

                if (distance <= request.Radius)
                    result.Points.Add(new MapPointViewModel(point));
            }

            var partners = PartnerService.GetAll().ToList();
            foreach (var partner in partners)
            {
                if (partner.Latitude == 0 || partner.Longitude == 0)
                    continue;

                var coordinate = new GeoCoordinate(partner.Latitude, partner.Longitude);
                var distance = center.GetDistanceTo(coordinate);

                if (distance <= request.Radius)
                    result.Partners.Add(PartnerToViewModel(partner));
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