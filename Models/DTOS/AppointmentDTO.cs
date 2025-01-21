using Server.Models.DTOs;

namespace Server.Models.DTOS
{
    public class AppointmentDTO
    {
        public required string ContactId { get; set; }
        public required string VisiteId { get; set; }
        public required LocationDto Location { get; set; }
    }
}