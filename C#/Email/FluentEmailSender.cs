using Framework.Notification.Email.Models;
using Framework.Notification.Email.Services;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Notification.Email
{
    public sealed class FluentEmailSender : IEmailSender
    {
        private readonly ILogger<FluentEmailSender> _logger;
        private readonly IHostingEnvironment _env;
        private IFluentEmail _fluentEmail;

        public FluentEmailSender(
            ILogger<FluentEmailSender> logger,
            IHostingEnvironment env,
            IFluentEmail fluentEmail)
        {
            _logger = logger;

            _env = env;
            _fluentEmail = fluentEmail;

        }

        public async Task<bool> SendEmailAsync(EmailOptions options, string htmlFileName = "", object razorParameter = null, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options), $"Value of \"{nameof(options)}\" can not be null");     
            
            if (!string.IsNullOrWhiteSpace(htmlFileName) && razorParameter is null)
                throw new ArgumentNullException(nameof(razorParameter), $"Value of \"{nameof(razorParameter)}\" can not be null");


            SendResponse? email;

            // if need to render razor page
            if (!string.IsNullOrWhiteSpace(htmlFileName))
            {
                string path = Path.Combine(_env.WebRootPath, "EmailTemplates", $"{htmlFileName}.cshtml");
                email = await _fluentEmail
                    .To(options.To.Email, options.To.FullName)
                    .Subject(options.Subject)
                    .UsingTemplateFromFile(path, razorParameter)
                    .SendAsync(cancellationToken);
            }
            else
                email = await _fluentEmail
                .To(options.To.Email, options.To.FullName)
                .Subject(options.Subject)
                .Body(options.Body)
                .SendAsync(cancellationToken);

            return email.Successful;
        }
    }
}
