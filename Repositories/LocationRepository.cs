using Microsoft.EntityFrameworkCore;
using Server.Interfaces.Repositories;
using Server.Data;
using Server.Models.Entities;

namespace Server.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)
        {
            return await _context.Locations
                .Where(l => l.Latitude == latitude && l.Longitude == longitude)
                .FirstOrDefaultAsync();
        }

        public async Task<Location> AddLocationAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            return location;
        }
    }
}
