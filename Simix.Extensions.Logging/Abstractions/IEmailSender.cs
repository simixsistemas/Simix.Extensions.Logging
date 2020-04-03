using System.Threading.Tasks;

namespace Simix.Extensions.Logging.Abstractions {
    public interface IEmailSender {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
