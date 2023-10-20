using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Planner.Model
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ImgUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/9/99/Sample_User_Icon.png";
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
