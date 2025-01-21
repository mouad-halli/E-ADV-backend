using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Interfaces.Repositories;
using Server.Interfaces.Services;
using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            ICurrentUserService currentUserService,
            IUserService userService,
            ILocationService locationService
        ) {
            _appointmentRepository = appointmentRepository;
            _currentUserService = currentUserService;
            _userService = userService;
            _locationService = locationService;
        }

        public async Task<bool> IsAppointmentVisitedAsync(string visiteId)
        {
            return await _appointmentRepository.IsAppointmentVisitedAsync(visiteId);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string id)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id);
        }

        public async Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId)
        {
            var userId =  _currentUserService.GetUserId();

            return await _appointmentRepository.GetAppointmentByVisiteIdAsync(visiteId, userId);
        }

        public async Task<Appointment> AddAppointmentAsync(AppointmentDTO appointmentData)
        {
            var user = await  _userService.getCurrentUser();

            var appointmentLocation = await _locationService.GetByCoordinatesAsync(appointmentData.Location.Latitude, appointmentData.Location.Longitude);

            if (appointmentLocation == null)
                appointmentLocation = await _locationService.AddLocationAsync(appointmentData.Location);


            var appointment = new Appointment
            {
                ContactId = appointmentData.ContactId,
                VisiteId = appointmentData.VisiteId,
                LocationId = appointmentLocation.Id,
                Location = appointmentLocation,
                User = user,
                UserId = user.Id
            };

            return await _appointmentRepository.AddAppointmentAsync(appointment);
        }
    }
}
