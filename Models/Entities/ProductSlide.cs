using System.Globalization;
using System.Text.Json.Serialization;

namespace Server.Models.Entities
{

    public enum FeedbackType
    {
        None = 0,
        Bad = 1,
        Neutral = 2,
        Good = 5
    }

    public class ProductSlide
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string ProductPresentationId { get; set; }
        [JsonIgnore]
        public ProductPresentation ProductPresentation { get; set; }
        public required string SlideId { get; set; } // external
        public string Comment { get; set; } = string.Empty;
        public FeedbackType Feedback { get; set; } = FeedbackType.None;
        public double TimeSpent { get; set; } = 0;
        // public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public required int OrderNumber { get; set; }
    }
}