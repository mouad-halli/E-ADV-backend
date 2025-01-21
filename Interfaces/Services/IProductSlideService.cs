using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Interfaces.Services
{
    public interface IProductSlideService
    {
        Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync();
        Task<ProductSlide> GetProductSlideByIdAsync(string id);
        Task<ProductSlide> UpdateProductSlideAsync(string slideId, UpdateProductSlideDTO productSlideData);
    }
}
