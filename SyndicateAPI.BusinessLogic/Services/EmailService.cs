using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendActivationMessage(string to, int code)
        {
            var message = new SendMailModel
            {
                MailTo = to,
                Subject = "[Syndicate] Активация",
                Message = $"Ваш код активации: {code}"
            };

            return await SMTPHelper.SendMail(message);
        }
    }
}
