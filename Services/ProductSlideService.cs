using Server.common.Exceptions;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Services
{
    public class ProductSlideService : IProductSlideService
    {
        private readonly IProductSlideRepository _productSlideRepository;
        private readonly ILogger<ProductSlideService> _logger;

        public ProductSlideService(
            IProductSlideRepository productSlideRepository,
            ILogger<ProductSlideService> logger
        )
        {
            _productSlideRepository = productSlideRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync()
        {
            return await _productSlideRepository.GetAllProductSlidesAsync();
        }

        public async Task<ProductSlide> GetProductSlideByIdAsync(string id)
        {
            return await _productSlideRepository.GetProductSlideByIdAsync(id);
        }

        public async Task<ProductSlide> UpdateProductSlideAsync(string slideId, UpdateProductSlideDTO productSlideData)
        {
            if (string.IsNullOrEmpty(slideId))
                throw new BadRequestException("slide id is required");
            // _logger.LogInformation("good 2");
            var existingSlide = await _productSlideRepository.GetProductSlideByIdAsync(slideId);
            if (existingSlide == null)
                throw new NotFoundException("slide not found");
            // _logger.LogInformation("good {comment}", productSlideData.Comment);
            existingSlide.Comment = productSlideData.Comment;
            existingSlide.TimeSpent = productSlideData.TimeSpent;
            existingSlide.Feedback = productSlideData.Feedback;
            existingSlide.UpdatedAt = DateTime.Parse(productSlideData.UpdatedAt);

            return await _productSlideRepository.UpdateProductSlideAsync(slideId, existingSlide);
        }
    }
}
