using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendPassword(string to, string password);
        Task<bool> SendActivationMessage(string to, int code);
        Task<bool> SendChangeMessage(string to, int code);
        Task<bool> SendSupportMessage(string from, string name, string message, bool isAuthenticated);
    }
}
