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
    [Route("api/partners")]
    [ApiController]
    [Authorize]
    public class PartnersController : Controller
    {
        private IUserService UserService { get; set; }
        private IPartnerService PartnerService { get; set; }
        private IPartnerProductService PartnerProductService { get; set; }
        private IFileService FileService { get; set; }

        public PartnersController([FromServices]
            IUserService userService,
            IPartnerService partnerService,
            IPartnerProductService partnerProductService,
            IFileService fileService)
        {
            UserService = userService;
            PartnerService = partnerService;
            PartnerProductService = partnerProductService;
            FileService = fileService;
        }

        private PartnerViewModel PartnerToViewModel(Partner partner)
        {
            var products = PartnerProductService.GetAll()
                .Where(x => x.Partner == partner)
                .ToList();

            return new PartnerViewModel(partner, products);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartners()
        {
            var partners = PartnerService.GetAll()
                .Select(x => PartnerToViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<PartnerViewModel>>
            {
                Data = partners
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPartnerByID(long id)
        {
            var partner = PartnerService.Get(id);
            if (partner == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Партнер не найден"
                });

            return Ok(new DataResponse<PartnerViewModel>
            {
                Data = PartnerToViewModel(partner)
            });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPartnerByName(string name)
        {
            var partner = PartnerService.GetAll()
                .FirstOrDefault(x => x.Name == name);

            if (partner == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Партнер не найден"
                });

            return Ok(new DataResponse<PartnerViewModel>
            {
                Data = PartnerToViewModel(partner)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePartner([FromBody] CreatePartnerRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var partner = PartnerService.GetAll()
                .FirstOrDefault(x => x.Name == request.Name);

            if (partner != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Партнер с таким именем уже существует"
                });

            var logo = FileService.Get(request.LogoID);
            if (logo == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Логотип не найден"
                });

            var icon = FileService.Get(request.MapIconID);
            if (icon == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Иконка не найдена"
                });

            partner = new Partner
            {
                Name = request.Name,
                Description = request.Description,
                Logo = logo,
                MapIcon = icon,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                MapPointType = request.MapPointType,
                Creator = user
            };

            PartnerService.Create(partner);

            foreach (var product in request.Products)
                PartnerProductService.Create(new PartnerProduct
                {
                    Name = product.Name,
                    Price = product.Price,
                    PointsCount = product.PointsCount,
                    Partner = partner
                });

            return Ok(new DataResponse<PartnerViewModel>
            {
                Data = PartnerToViewModel(partner)
            });
        }
    }
}