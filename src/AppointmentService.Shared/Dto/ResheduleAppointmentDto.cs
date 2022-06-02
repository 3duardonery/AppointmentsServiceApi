using System;

namespace AppointmentService.Shared.Dto
{
    public sealed class ResheduleAppointmentDto
    {
        public string AppointmentId { get; set; }
        public string ServiceId { get; set; }
        public string CustomerId { get; set; }
        public string Time { get; set; }
        public DateTime Date { get; set; }
    }
}
