using System.ComponentModel.DataAnnotations;

namespace Notification.Email.Models
{
    public record EmailAddress([EmailAddress] string Email, string? FullName = null);
}
