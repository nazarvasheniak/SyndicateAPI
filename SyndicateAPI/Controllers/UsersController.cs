﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace Gold.IO.Exchange.API.EthereumRPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserService UserService { get; set; }
        private IPersonService PersonService { get; set; }
        private ICityService CityService { get; set; }
        private IEmailService EmailService { get; set; }

        public UsersController([FromServices]
            IUserService userService,
            IPersonService personService,
            ICityService cityService,
            IEmailService emailService)
        {
            UserService = userService;
            PersonService = personService;
            CityService = cityService;
            EmailService = emailService;
        }

        [HttpPost("reg")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
        {
            // TO-DO: Вынести в сервис 
            var city = CityService.Get(request.CityID);
            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City error"
                });

            var user = UserService.GetAll().FirstOrDefault(x => x.Login == request.Email);
            if (user != null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Email is already used by another user"
                });

            user = UserService.GetAll().FirstOrDefault(x => x.Nickname == request.Nickname);
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
                FirstName = request.FirsName,
                LastName = request.LastName,
                Email = request.Email,
                City = city
            };

            PersonService.Create(person);

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

            UserService.Create(user);

            await EmailService.SendActivationMessage(user.Login, user.ActivationCode);

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

            return Ok();
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorization([FromBody] AuthorizationRequest request)
        {
            var authToken = UserService.AuthorizeUser(request.Login, request.Password);
            if (authToken == null)
                return Json(new ResponseModel { Success = false, Message = "Wrong username or password" });

            var user = UserService.GetAll().FirstOrDefault(x => x.Login == request.Login);

            return Json(new AuthorizationResponse { Token = authToken, User = new UserViewModel(user) });
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}