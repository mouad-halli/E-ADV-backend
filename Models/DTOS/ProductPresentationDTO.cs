using Server.Models.DTOs;
using Server.Models.Entities;

namespace Server.Models.DTOS
{
    public class FindProductPresentationDTO
    {
        public string DoctorId { get; set; }
        public string ProductId { get; set; }
    }

    public class ProductPresentationDTO {
        public required string ProductId { get; set; }
        public List<ProductSlideDTO> ProductSlides { get; set; } = [];
    }

    public class GetProductPresentationQueryParams {
        public required string ProductId { get; set; }
        public string? VisiteId { get; set; }
    }

    public class ProductPresentationResponse
    {
        public string latestPresentationDate { get; set; }
        public double feedback { get; set; }
        public string presentationStatus { get; set; }
    }

    public class UpdateProductPresentationDTO
    {
        public string GeneralComment { get; set; }
        public ProductPresentationFeedbackType GeneralFeedback { get; set; }
    }
}