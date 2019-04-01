using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyndicateAPI.Utils.Helpers
{
    public static class SMTPHelper
    {
        public static string SMTP_SERVER = "smtp.gmail.com";
        public static int SMTP_PORT = 587;
        public static string SMTP_LOGIN = "nazar.vasheniak@gmail.com";
        public static string SMTP_PASSWORD = "82356123neron";

        public static void SendMailMX()
        {
            
        }

        /// <summary>
        /// Send Mail via SMTP method
        /// </summary>
        /// <param name="model">SendMailModel contents all required params for sending mail</param>
        public static async Task<bool> SendMail(SendMailModel model)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SMTP_LOGIN);
                mail.To.Add(new MailAddress(model.MailTo));
                mail.Subject = model.Subject;
                mail.IsBodyHtml = true;
                mail.Body = model.Message;

                if (!string.IsNullOrEmpty(model.AttachFile))
                    mail.Attachments.Add(new Attachment(model.AttachFile));

                SmtpClient client = new SmtpClient(SMTP_SERVER, SMTP_PORT);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SMTP_LOGIN, SMTP_PASSWORD);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                await client.SendMailAsync(mail);
                mail.Dispose();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format(@"Error message: {0}", e.Message));
                return false;
            }
        }

        static Regex ValidEmailRegex = CreateValidEmailRegex();

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        public static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
    }

    /// <summary>
    /// Model for SMTP mail sending properties
    /// </summary>
    public class SendMailModel
    {
        /// <summary>
        /// SMTP server address
        /// </summary>
        //public string SmtpServer { get; set; }
        /// <summary>
        /// SMTP Server Port
        /// </summary>
        //public int SmtpServerPort { get; set; }
        /// <summary>
        /// SMTP server password
        /// </summary>
        //public string Password { get; set; }
        /// <summary>
        /// E-mail sender address
        /// </summary>
        //public string From { get; set; }
        /// <summary>
        /// E-mail receiver address
        /// </summary>
        public string MailTo { get; set; }
        /// <summary>
        /// E-mail subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// E-mail body message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// E-mail attach file content
        /// </summary>
        public string AttachFile { get; set; }
    }
}
