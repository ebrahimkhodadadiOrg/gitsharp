
using Framework.Notification.Email.Options;
using Framework.Notification.Email.Services;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Email;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Config
{
    public static class NotificationExtension
    {
        public static void AddNotificaitons(this IServiceCollection services, IConfiguration configuration)
        {
            var _options = configuration
                .GetSection(EmailSenderOptions.Section)
                .Get<EmailSenderOptions>();

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(
                _options.SmtpUser.Email,
                _options.SmtpPassword);

            smtpClient.Host = _options.SmtpHost;
            smtpClient.Port = _options.SmtpPort;
            smtpClient.EnableSsl = _options.SmtpSsl;
            
            services
                .AddFluentEmail(_options.SmtpUser.Email, _options.SmtpUser.FullName)
                .AddRazorRenderer()
                .AddSmtpSender(smtpClient);


            services.AddSingleton<IEmailSender, FluentEmailSender>();
        }
    }
}
