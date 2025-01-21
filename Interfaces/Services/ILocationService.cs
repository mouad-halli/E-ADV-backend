using Server.Models.DTOs;
using Server.Models.Entities;

namespace Server.Interfaces.Services
{
    public interface ILocationService
    {
        Task<Location?> GetByCoordinatesAsync(double latitude, double longitude);
        Task<Location> AddLocationAsync(LocationDto location);
    }
}
