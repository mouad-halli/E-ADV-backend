
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{
    public class Appointment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public required User User { get; set; }
        public required string ContactId { get; set; } // external
        public required string VisiteId { get; set; } // external
        public required string LocationId { get; set; }
        public required Location Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public ICollection<ProductPresentation>? ProductPresentations { get; set; }
    }
}