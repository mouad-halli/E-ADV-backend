using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<bool> IsAppointmentVisitedAsync(string visiteId);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId);
        Task<Appointment> GetAppointmentByIdAsync(string id);
        Task<Appointment> AddAppointmentAsync(AppointmentDTO appointmentData);
    }
}
