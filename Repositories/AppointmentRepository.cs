using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces.Repositories;
using Server.Models.Entities;

namespace Server.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAppointmentVisitedAsync(string visiteId)
        {
            return await _context.Set<Appointment>()
                    .Where(a => a.VisiteId == visiteId)
                    .Select(a => a.ProductPresentations
                        .Any(pp => pp.ProductSlides.Any(ps => ps.TimeSpent > 3)))
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId, string userId)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.VisiteId == visiteId && a.UserId == userId);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(string id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            var createdAppointment = await _context.Appointments.AddAsync(appointment);
            
            await _context.SaveChangesAsync();

            return createdAppointment.Entity;
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(string id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
