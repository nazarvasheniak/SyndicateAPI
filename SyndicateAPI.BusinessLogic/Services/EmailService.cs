using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.Utils.Helpers;
using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendPassword(string to, string password)
        {
            var message = new SendMailModel
            {
                MailTo = to,
                Subject = "[Syndicate] Восстановление аккаунта",
                Message = $"Ваш новый пароль: {password}"
            };

            return await SMTPHelper.SendMail(message);
        }

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

        public async Task<bool> SendChangeMessage(string to, int code)
        {
            var message = new SendMailModel
            {
                MailTo = to,
                Subject = "[Syndicate] Активация",
                Message = $"Подтвердите изменение Email.<br>Ваш код подтверждения: {code}"
            };

            return await SMTPHelper.SendMail(message);
        }

        public async Task<bool> SendSupportMessage(string from, string name, string msg, bool isAuthenticated)
        {
            string isAuthenticatedStr;
            if (isAuthenticated)
                isAuthenticatedStr = "Да";
            else
                isAuthenticatedStr = "Нет";

            var message = new SendMailModel
            {
                MailTo = "zks@d-syndicate.ru",
                Subject = "[Syndicate] Сообщение из формы обратной связи",
                Message = $"Email отправителя: {from}<br>Имя отправителя: {name}<br>Авторизован: {isAuthenticatedStr}<br>Сообщение: {msg}"
            };

            return await SMTPHelper.SendMail(message);
        }
    }
}
