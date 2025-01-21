using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.DTOs;
using Server.Models.Entities;

namespace Server.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)
        {
            return await _locationRepository.GetByCoordinatesAsync(latitude, longitude);
        }

        public async Task<Location> AddLocationAsync(LocationDto location)
        {
            var newLocation = new Location
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };

            return await _locationRepository.AddLocationAsync(newLocation);
        }
    }
}
