using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces.Repositories;
using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Repositories
{
    public class ProductPresentationRepository : IProductPresentationRepository
    {
        private readonly AppDbContext _context;

        public ProductPresentationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter)
        {
            var query = _context.ProductPresentations
                .Include(p => p.ProductSlides)
                .Include(p => p.Appointment)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.ProductId))
            {
                query = query.Where(p => p.ProductId == filter.ProductId);
            }

            if (!string.IsNullOrEmpty(filter.VisiteId))
            {
                query = query.Where(p => p.Appointment.VisiteId == filter.VisiteId);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProductPresentation?> GetLatestUserProductPresentation(string productId, string doctorId, string userId)
        {
            // finds the user's latest presentation with a doctor and populates productSlides and appointment
            return await _context.ProductPresentations
                .Include(p => p.ProductSlides)
                .Include(p => p.Appointment)
                .Where(p => p.ProductId == productId && p.Appointment.ContactId == doctorId && p.Appointment.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync()
        {
            return await _context.ProductPresentations.ToListAsync();
        }

        public async Task<ProductPresentation> GetProductPresentationByIdAsync(string id)
        {
            return await _context.ProductPresentations.FindAsync(id);
        }

        public async Task<ProductPresentation> AddProductPresentationAsync(ProductPresentation productPresentation)
        {
            var createdProductPresentation = await _context.ProductPresentations.AddAsync(productPresentation);
            await _context.SaveChangesAsync();
            return createdProductPresentation.Entity;
        }

        public async Task UpdateProductPresentationAsync(string id, UpdateProductPresentationDTO productPresentation)
        {
            var existingProductPresentation = await _context.ProductPresentations.FindAsync(id);
            if (existingProductPresentation != null)
            {
                if (!string.IsNullOrEmpty(productPresentation.GeneralComment))
                    existingProductPresentation.GeneralComment = productPresentation.GeneralComment;
                    existingProductPresentation.GeneralFeedback = productPresentation.GeneralFeedback;
            }
            // _context.ProductPresentations.Update(productPresentation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductPresentationAsync(string id)
        {
            var product = await _context.ProductPresentations.FindAsync(id);
            if (product != null)
            {
                _context.ProductPresentations.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
