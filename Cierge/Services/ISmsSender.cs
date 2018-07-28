using System.Threading.Tasks;

namespace Cierge.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
