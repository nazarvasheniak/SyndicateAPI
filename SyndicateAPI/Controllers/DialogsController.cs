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

        private DialogViewModel GetDialogWithMessages(Dialog dialog)
        {
            var messages = DialogMessageService.GetAll()
                .Where(x => x.Dialog == dialog)
                .ToList();

            var result = new DialogViewModel(dialog, messages);

            return result;
        }

        private List<DialogViewModel> GetDialogWithMessages(IEnumerable<Dialog> dialogs)
        {
            var result = new List<DialogViewModel>();
            
            foreach (var dialog in dialogs)
            {
                var messages = DialogMessageService.GetAll()
                    .Where(x => x.Dialog == dialog)
                    .ToList();

                result.Add(new DialogViewModel(dialog, messages));
            }

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
                .Select(x => GetDialogWithMessages(x))
                .ToList();

            return Ok(new DataResponse<List<DialogViewModel>>
            {
                Data = dialogs
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