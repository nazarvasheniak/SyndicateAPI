using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

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

        [HttpPost("reg")]
        public IActionResult Registration([FromBody] RegistrationRequest request)
        {
            var city = CityService.Get(request.CityID);
            if (city == null)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "City error"
                });

            if (UserService.IsEmailExist(request.Email))
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Email is already used by another user"
                });

            if (UserService.IsNicknameExist(request.Nickname))
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

            var person = PersonService.CreatePerson(request.FirstName, request.LastName, request.Email, city);
            var user = UserService.CreateUser(request.Nickname, request.Password, person);

            var activationMessage = EmailService.SendActivationMessage(user.Login, user.ActivationCode);
            activationMessage.Wait();

            if (!activationMessage.Result)
                return Ok(new ResponseModel
                {
                    Success = false,
                    Message = "Ошибка отправки активационного письма"
                });

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
                    Message = "Неверный логин или пароль"
                });

            var user = AdminUserService.GetAll()
                .FirstOrDefault(x => x.Login == request.Login);

            return Ok(new AdminAuthorizationResponse
            {
                Token = authToken
            });
        }

        [HttpPost("online")]
        [Authorize]
        public async Task<IActionResult> SetOnlineStatus([FromBody] SetOnlineStatusRequest request)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            user.IsOnline = request.IsOnline;
            if (request.IsOnline)
            {
                user.Latitude = request.Latitude;
                user.Longitude = request.Longitude;
            }

            UserService.Update(user);

            return Ok(new DataResponse<UserViewModel>
            {
                Data = new UserViewModel(user)
            });
        }

        [HttpGet("online")]
        [Authorize]
        public async Task<IActionResult> GetOnlineStatus()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            return Ok(new GetOnlineStatusResponse
            {
                IsOnline = user.IsOnline,
                Latitude = user.Latitude,
                Longitude = user.Longitude
            });
        }

        [HttpGet("{id}/online")]
        [Authorize]
        public async Task<IActionResult> GetOnlineStatus(long id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return Ok(new ResponseModel
                {
                    Success = false,
                    Message = "User not found"
                });

            return Ok(new GetOnlineStatusResponse
            {
                IsOnline = user.IsOnline,
                Latitude = user.Latitude,
                Longitude = user.Longitude
            });
        }

        private int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
    }
}