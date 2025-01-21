using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories
{
    public class ProductSlideRepository : IProductSlideRepository
    {
        private readonly AppDbContext _context;

        public ProductSlideRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync()
        {
            return await _context.ProductSlides.ToListAsync();
        }

        public async Task<ProductSlide> GetProductSlideByIdAsync(string id)
        {
            return await _context.ProductSlides.FindAsync(id);
        }

        public async Task AddProductSlideAsync(ProductSlide productSlide)
        {
            await _context.ProductSlides.AddAsync(productSlide);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductSlide> UpdateProductSlideAsync(string slideId, ProductSlide productSlide)
        {
            var updatedSlide = _context.ProductSlides.Update(productSlide);
            await _context.SaveChangesAsync();
            return updatedSlide.Entity;
        }

        public async Task DeleteProductSlideAsync(string id)
        {
            var productSlide = await _context.ProductSlides.FindAsync(id);
            if (productSlide != null)
            {
                _context.ProductSlides.Remove(productSlide);
                await _context.SaveChangesAsync();
            }
        }
    }
}
