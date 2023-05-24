
namespace Notification.Email.Models
{
    public class EmailOptions
    {
        public EmailAddress To { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
