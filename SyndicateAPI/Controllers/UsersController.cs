﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;
using SyndicateAPI.Utils.Helpers;

namespace Gold.IO.Exchange.API.EthereumRPC.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService UserService { get; set; }
        private IUserTempService UserTempService { get; set; }
        private IAdminUserService AdminUserService { get; set; }
        private IPersonService PersonService { get; set; }
        private ICityService CityService { get; set; }
        private IEmailService EmailService { get; set; }

        public UsersController([FromServices]
            IUserService userService,
            IUserTempService userTempService,
            IAdminUserService adminUserService,
            IPersonService personService,
            ICityService cityService,
            IEmailService emailService)
        {
            UserService = userService;
            UserTempService = userTempService;
            AdminUserService = adminUserService;
            PersonService = personService;
            CityService = cityService;
            EmailService = emailService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestMail()
        {
            SMTPHelper.SendMailMX();
            return Ok();
        }

        [HttpPost("reg")]
        public IActionResult Registration([FromBody] RegistrationRequest request)
        {
            // TO-DO: Вынести в сервис 
            var city = CityService.Get(request.CityID);
            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City error"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == request.Email);

            if (user != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Email is already used by another user"
                });

            user = UserService.GetAll()
                .FirstOrDefault(x => x.Nickname == request.Nickname);

            if (user != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Nickname is already used by another user"
                });

            if (!request.Password.Equals(request.ConfirmPassword))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Passwords is not equals"
                });

            var person = new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                City = city
            };

            user = new User
            {
                Login = request.Email,
                Password = request.Password,
                Nickname = request.Nickname,
                RegTime = DateTime.UtcNow,
                PointsCount = 0,
                Person = person,
                IsActive = false,
                ActivationCode = RandomNumber()
            };

            var activationMessage = EmailService.SendActivationMessage(user.Login, user.ActivationCode);
            activationMessage.Wait();

            if (!activationMessage.Result)
                return Ok(new ResponseModel
                {
                    Success = false,
                    Message = "Ошибка отправки активационного письма"
                });

            try
            {
                PersonService.Create(person);
                UserService.Create(user);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Unique login exception"
                });
            }

            return Ok(new RegistrationResponse
            {
                UserID = user.ID
            });
        }

        [HttpPost("activation")]
        public async Task<IActionResult> Activation([FromBody] ActivationRequest request)
        {
            var user = UserService.Get(request.UserID);
            if (user == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            if (!request.Code.Equals(user.ActivationCode))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Activation code invalid"
                });

            user.IsActive = true;
            user.ActivationTime = DateTime.UtcNow;
            UserService.Update(user);

            return Ok(new ResponseModel());
        }

        [HttpPost("activation/resend")]
        public async Task<IActionResult> ResendActivation([FromBody] ResendActivationRequest request)
        {
            var user = UserService.Get(request.UserID);
            if (user == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            if (user.IsActive)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "User already activated"
                });

            user.ActivationCode = RandomNumber();
            UserService.Update(user);

            await EmailService.SendActivationMessage(user.Login, user.ActivationCode);

            return Ok(new ResponseModel());
        }

        [HttpPost("changeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ActivationRequest request)
        {
            var user = UserService.Get(request.UserID);
            if (user == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            var temp = UserTempService.GetAll()
                .FirstOrDefault(x => x.User == user);

            if (temp == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Change email request not found"
                });

            if (!request.Code.Equals(temp.TempCode))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Вы ввели неверный код"
                });

            user.Login = temp.Email;
            UserService.Update(user);

            user.Person.Email = temp.Email;
            PersonService.Update(user.Person);

            return Ok(new ResponseModel());
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorization([FromBody] AuthorizationRequest request)
        {
            var authToken = UserService.AuthorizeUser(request.Login, request.Password);
            if (authToken == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Wrong username or password"
                });

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.Login == request.Login);

            return Ok(new AuthorizationResponse
            {
                Token = authToken,
                User = new UserViewModel(user)
            });
        }

        [HttpPost("admin/auth")]
        public async Task<IActionResult> AdminAuthorization([FromBody] AuthorizationRequest request)
        {
            var authToken = AdminUserService.AuthorizeUser(request.Login, request.Password);
            if (authToken == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Wrong username or password"
                });

            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == request.Login);

            return Ok(new AdminAuthorizationResponse
            {
                Token = authToken,
                User = new AdminUserViewModel(user)
            });
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}