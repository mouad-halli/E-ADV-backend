using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.Entities;

namespace Server.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<bool> IsAppointmentVisitedAsync(string visiteId);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId, string userId);
        Task<Appointment> GetAppointmentByIdAsync(string id);
        Task<Appointment> AddAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(string id);
    }
}
