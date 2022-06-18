using System;
using System.Collections.Generic;

namespace AppointmentService.Shared.Dto
{
    public sealed class OpenBookRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<string> ServiceIds { get; set; }
        public int Duration { get; set; }
        public string ProfessionalId { get; set; }
    }
}
