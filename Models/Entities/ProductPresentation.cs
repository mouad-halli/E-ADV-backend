namespace Server.Models.Entities
{
        public enum ProductPresentationFeedbackType
    {
        None = 0,
        Bad = 1,
        Neutral = 2,
        Good = 3
    }

    public class ProductPresentation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string AppointmentId { get; set; }
        public required Appointment Appointment { get; set; }
        public required string ProductId { get; set; }
        public ICollection<ProductSlide>? ProductSlides { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string GeneralComment { get; set; } = string.Empty;
        public ProductPresentationFeedbackType GeneralFeedback { get; set; } = ProductPresentationFeedbackType.None;
    }
}