using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Interfaces.Services
{
    public interface IProductPresentationService
    {

        Task<ProductPresentation> FindUserProductPresentation(FindProductPresentationDTO data);
        Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync();
        Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter);
        Task<ProductPresentation> GetProductPresentationByIdAsync(string id);
        Task<ProductPresentation> CreateProductPresentationtAsync(ProductPresentationDTO data, string visiteId);
        Task UpdateProductPresentationtAsync(string id, UpdateProductPresentationDTO productPresentation);
    }
}
