using System;

namespace AppointmentService.Shared.Dto
{
    public sealed class CancelBookRequest
    {
        public string BookId { get; set; }
        public DateTime CancelDate { get; set; }
        public string CancelBy { get; set; }
        public string Reason { get; set; }
    }
}
