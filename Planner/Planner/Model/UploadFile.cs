using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Planner.Model
{
    public class UploadFile
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required]
        public int WorkTaskId { get; set; } = 0;
        [JsonIgnore]
        public WorkTask WorkTask { get; set; }

    }
}
