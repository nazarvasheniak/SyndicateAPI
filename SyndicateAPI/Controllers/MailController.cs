using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : Controller
    {
        private IUserService UserService { get; set; }
        private IEmailService EmailService { get; set; }

        public MailController([FromServices]
            IUserService userService,
            IEmailService emailService)
        {
            UserService = userService;
            EmailService = emailService;
        }

        [HttpPost("support")]
        public async Task<IActionResult> SendSupportMessage([FromBody] SendSupportMessageRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            if (User.Identity.IsAuthenticated)
                await EmailService.SendSupportMessage(user.Person.Email, user.Person.FirstName, request.Message, true);
            else
                await EmailService.SendSupportMessage(request.Email, request.Name, request.Message, false);

            return Ok(new ResponseModel());
        }
    }
}