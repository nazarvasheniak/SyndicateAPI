using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;

namespace SyndicateAPI.Controllers.Admin
{
    [Route("api/admin/vehicles")]
    [ApiController]
    [Authorize]
    public class VehiclesController : Controller
    {
        private IAdminUserService AdminUserService { get; set; }
        private IVehicleService VehicleService { get; set; }
    }
}