using System;

namespace AppointmentService.Shared.ViewModels
{
    public sealed class AppointmentViewModel
    {

        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public bool Executed { get; set; }
        public bool IsCancelled { get; set; }
        public string CustomerId { get; set; }
        public ProfessionalBookViewModel ProfessionalReference { get; set; }
        public ServiceBookViewModel ServiceReference { get; set; }
    }
}
