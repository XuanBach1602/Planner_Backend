using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Planner.Model
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; } = "";
        //public string Address { get; set; } = "";
        public string ImgUrl { get; set; } = "default.png";
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
    }
}
