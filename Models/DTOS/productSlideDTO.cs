using Server.Models.Entities;

namespace Server.Models.DTOS
{
    public class ProductSlideDTO
    {
        public required string SlideId { get; set; }
        public string Comment { get; set; }
        public FeedbackType Feedback { get; set; }
        public double TimeSpent { get; set; }
        public int OrderNumber { get; set; }
    }

    public class UpdateProductSlideDTO
    {
        public string Comment { get; set; }
        public FeedbackType Feedback { get; set; }
        public double TimeSpent { get; set; }
        public string UpdatedAt { get; set; }
    }
}