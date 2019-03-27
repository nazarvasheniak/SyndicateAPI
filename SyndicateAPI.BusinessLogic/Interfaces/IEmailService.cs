using System.Threading.Tasks;

namespace SyndicateAPI.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendActivationMessage(string to, int code);
        Task<bool> SendChangeMessage(string to, int code);
    }
}
