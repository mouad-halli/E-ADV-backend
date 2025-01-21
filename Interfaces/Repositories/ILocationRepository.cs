using Server.Models.Entities;

namespace Server.Interfaces.Repositories
{
    public interface ILocationRepository
    {
        Task<Location?> GetByCoordinatesAsync(double latitude, double longitude);
        Task<Location> AddLocationAsync(Location location);
    }
}
