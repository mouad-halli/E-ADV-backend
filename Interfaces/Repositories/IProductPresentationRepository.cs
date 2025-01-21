using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Interfaces.Repositories
{
    public interface IProductPresentationRepository
    {
        Task<ProductPresentation?> GetLatestUserProductPresentation(string productId, string doctorId, string userId);
        Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync();
        Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter);
        Task<ProductPresentation> GetProductPresentationByIdAsync(string id);
        Task<ProductPresentation> AddProductPresentationAsync(ProductPresentation productPresentation);
        Task UpdateProductPresentationAsync(string id, UpdateProductPresentationDTO productPresentation);
        Task DeleteProductPresentationAsync(string id);
    }
}
