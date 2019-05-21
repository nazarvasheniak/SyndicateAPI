using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;

namespace SyndicateAPI.Controllers
{
    [Route("api/dialogs")]
    [ApiController]
    [Authorize]
    public class DialogsController : Controller
    {
        private IUserService UserService { get; set; }
        private IDialogService DialogService { get; set; }
        private IDialogMessageService DialogMessageService { get; set; }

        public DialogsController([FromServices]
            IUserService userService,
            IDialogService dialogService,
            IDialogMessageService dialogMessageService)
        {
            UserService = userService;
            DialogService = dialogService;
            DialogMessageService = dialogMessageService;
        }

        private DialogViewModel DialogToViewModel(Dialog dialog)
        {
            var lastMessage = DialogMessageService.GetAll()
                .AsEnumerable()
                .LastOrDefault(x => x.Dialog == dialog);

            var result = new DialogViewModel(dialog, lastMessage);

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyDialogs()
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var dialogs = DialogService.GetAll()
                .Where(x =>
                    x.Participant1 == user ||
                    x.Participant2 == user)
                .OrderByDescending(x => x.StartDate)
                .Select(x => DialogToViewModel(x))
                .ToList();

            return Ok(new DataResponse<List<DialogViewModel>>
            {
                Data = dialogs
            });
        }

        [HttpGet("{dialogID}/messages")]
        public async Task<IActionResult> GetMessagesByDialogId([FromQuery] GetListRequest request, long dialogID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var dialog = DialogService.Get(dialogID);
            if (dialog == null)
                return Ok(new DataResponse<List<DialogMessageViewModel>>
                {
                    Data = new List<DialogMessageViewModel>()
                });

            if (dialog.Participant1 != user && dialog.Participant2 != user)
                return Forbid();

            var messages = DialogMessageService.GetAll()
                .Where(x => x.Dialog == dialog)
                .ToList();

            foreach (var message in messages)
                if (!message.IsReaded)
                {
                    message.IsReaded = true;
                    DialogMessageService.Update(message);
                }

            var result = messages
                .OrderByDescending(x => x.Time)
                .Select(x => new DialogMessageViewModel(x))
                .Skip((request.PageNumber - 1) * request.PageCount)
                .Take(request.PageCount)
                .ToList();

            return Ok(new ListResponse<DialogMessageViewModel>
            {
                Data = result,
                Pagination = new Pagination(messages.Count, request.PageNumber, request.PageCount)
            });
        }

        [HttpGet("participants/{participantID}/messages")]
        public async Task<IActionResult> GetMessagesByParticipantId([FromQuery] GetListRequest request, long participantID)
        {
            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var participant = UserService.Get(participantID);
            if (participant == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не найден"
                });

            var dialog = DialogService.GetAll()
                .FirstOrDefault(x =>
                    x.Participant1 == participant ||
                    x.Participant2 == participant);

            if (dialog == null)
                return Ok(new DataResponse<List<DialogMessageViewModel>>
                {
                    Data = new List<DialogMessageViewModel>()
                });

            if (dialog.Participant1 != user && dialog.Participant2 != user)
                return Forbid();

            var messages = DialogMessageService.GetAll()
                .Where(x => x.Dialog == dialog)
                .ToList();

            foreach (var message in messages)
                if (!message.IsReaded)
                {
                    message.IsReaded = true;
                    DialogMessageService.Update(message);
                }

            var result = messages
                .OrderByDescending(x => x.Time)
                .Select(x => new DialogMessageViewModel(x))
                .Skip((request.PageNumber - 1) * request.PageCount)
                .Take(request.PageCount)
                .ToList();

            return Ok(new ListResponse<DialogMessageViewModel>
            {
                Data = result,
                Pagination = new Pagination(messages.Count, request.PageNumber, request.PageCount)
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendDialogMessageRequest request)
        {
            var now = DateTime.UtcNow;

            var user = UserService.GetAll()
                .FirstOrDefault(x => x.ID.ToString() == User.Identity.Name);

            var recepient = UserService.Get(request.RecipientID);
            if (recepient == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не найден"
                });

            var dialog = DialogService.GetAll()
                .FirstOrDefault(x =>
                    x.Participant1 == user ||
                    x.Participant2 == user ||
                    x.Participant1 == recepient ||
                    x.Participant2 == recepient);

            if (dialog == null)
                DialogService.Create(dialog = new Dialog
                {
                    Participant1 = user,
                    Participant2 = recepient,
                    StartDate = now
                });

            DialogMessageService.Create(new DialogMessage
            {
                Dialog = dialog,
                Sender = user,
                Content = request.Content,
                Time = now,
                IsReaded = false
            });

            return Ok(new ResponseModel());
        }
    }
}