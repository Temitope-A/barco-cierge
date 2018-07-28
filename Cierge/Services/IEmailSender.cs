using System.Threading.Tasks;

namespace Cierge.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
