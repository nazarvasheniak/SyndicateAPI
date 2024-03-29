﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Domain.Enums;
using SyndicateAPI.Domain.Models;
using SyndicateAPI.Models;
using SyndicateAPI.Models.Request;
using SyndicateAPI.Models.Response;
using SyndicateAPI.WebSocketManager;

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
        private NotificationsMessageHandler Notifications { get; set; }

        public DialogsController([FromServices]
            IUserService userService,
            IDialogService dialogService,
            IDialogMessageService dialogMessageService,
            NotificationsMessageHandler notifications)
        {
            UserService = userService;
            DialogService = dialogService;
            DialogMessageService = dialogMessageService;
            Notifications = notifications;
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
                .Where(x => x.FromUser == user || x.ToUser == user)
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

            var messages = DialogMessageService.GetAll()
                .Where(x => x.Dialog == dialog)
                .ToList();

            foreach (var message in messages)
                if (!message.IsReaded && message.Sender != user)
                {
                    message.IsReaded = true;
                    DialogMessageService.Update(message);
                }

            var result = messages
                .OrderByDescending(x => x.Time)
                .Select(x => new DialogMessageViewModel(x, x.Dialog.FromUser == user))
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
                .FirstOrDefault(x => x.ToUser == participant && x.FromUser == user);

            if (dialog == null)
                dialog = DialogService.GetAll()
                    .FirstOrDefault(x => x.ToUser == user && x.FromUser == participant);

            if (dialog == null)
                return Ok(new DataResponse<List<DialogMessageViewModel>>
                {
                    Data = new List<DialogMessageViewModel>()
                });

            var messages = DialogMessageService.GetAll()
                .Where(x => x.Dialog == dialog)
                .ToList();

            foreach (var message in messages)
                if (!message.IsReaded && message.Sender != user)
                {
                    message.IsReaded = true;
                    DialogMessageService.Update(message);
                }

            var result = messages
                .OrderByDescending(x => x.Time)
                .Select(x => new DialogMessageViewModel(x, x.Dialog.FromUser == user))
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

            if (user.ID == request.RecipientID)
                return BadRequest(new ResponseModel
                {
                    Success = false,
                    Message = "Нельзя отправлять сообщения самому себе"
                });

            var recepient = UserService.Get(request.RecipientID);
            if (recepient == null)
                return NotFound(new ResponseModel
                {
                    Success = false,
                    Message = "Пользователь не найден"
                });

            var dialog = DialogService.GetAll()
                .FirstOrDefault(x => x.ToUser == recepient && x.FromUser == user);

            if (dialog == null)
                dialog = DialogService.GetAll()
                    .FirstOrDefault(x => x.ToUser == user && x.FromUser == recepient);

            if (dialog == null)
                DialogService.Create(dialog = new Dialog
                {
                    FromUser = user,
                    ToUser = recepient,
                    StartDate = now
                });

            var message = new DialogMessage
            {
                Dialog = dialog,
                Type = DialogMessageType.Outgoing,
                Sender = user,
                Content = request.Content,
                Time = now,
                IsReaded = false
            };

            DialogMessageService.Create(message);

            await Notifications.SendUpdateToUser(recepient.ID, SocketMessageType.PrivateMessage,
                new { dialogID = dialog.ID });

            return Ok(new ResponseModel());
        }
    }
}