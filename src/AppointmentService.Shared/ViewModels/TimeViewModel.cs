namespace AppointmentService.Shared.ViewModels
{
    public sealed class TimeViewModel
    {
        public string Id { get; set; }
        public string AvailableHour { get; set; }
        public string ProfessionalId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsCancelled { get; set; }
    }
}
