using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Planner.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [ForeignKey("Plans")]
        public int PlanID { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(PlanID))]
        public Plan? Plan { get; set; }
    }
}
