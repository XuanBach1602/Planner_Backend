using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public bool IsPrivacy { get; set; } = true;

        [ForeignKey("Users")]
        public string CreatedUserID { get; set; } = string.Empty;

        //[JsonIgnore]
        //public User? User { get; set; }

    }
}