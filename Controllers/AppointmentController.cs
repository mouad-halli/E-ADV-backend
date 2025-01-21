using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Interfaces.Services;
using Server.Models.DTOs;
using Server.Models.DTOS;
using Server.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            IAppointmentService appointmentService,
            ILogger<AppointmentController> logger
        ) {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("visite/{visiteId}")]
        public async Task<ActionResult<Appointment>> GetAppointmentByVisiteId(string visiteId)
        {
            var appointment = await _appointmentService.GetAppointmentByVisiteIdAsync(visiteId);

            return Ok(appointment);
        }

        [HttpGet("isVisited/{visiteId}")]
        public async Task<ActionResult<Appointment>> GetIsAppointmentVisited(string visiteId)
        {
            var isVisited = await _appointmentService.IsAppointmentVisitedAsync(visiteId);

            return Ok(isVisited);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(string id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult> AddAppointment([FromBody] AppointmentDTO appointmentData)
        {
            if (appointmentData == null || !ModelState.IsValid)
                return BadRequest(ModelState);
        
            var appointment = await _appointmentService.AddAppointmentAsync(appointmentData);
        
            return Ok(appointment);
        }
    }
}
