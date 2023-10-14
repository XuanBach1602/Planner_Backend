using System.ComponentModel.DataAnnotations;

namespace Planner.Model
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
