using Framework.Notification.Email.Models;

namespace Framework.Notification.Email.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailOptions options, string htmlFileName = "", object razorParameter = null, CancellationToken cancellationToken = default);
    }
}
