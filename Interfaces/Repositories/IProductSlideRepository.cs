using Server.Models.Entities;

namespace Server.Interfaces.Repositories
{
    public interface IProductSlideRepository
    {
        Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync();
        Task<ProductSlide> GetProductSlideByIdAsync(string id);
        Task AddProductSlideAsync(ProductSlide productSlide);
        Task<ProductSlide> UpdateProductSlideAsync(string slideId, ProductSlide productSlide);
        Task DeleteProductSlideAsync(string id);
    }
}
