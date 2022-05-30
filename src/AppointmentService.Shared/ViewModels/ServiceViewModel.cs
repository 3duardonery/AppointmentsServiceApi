namespace AppointmentService.Shared.ViewModels
{
    public sealed class ServiceViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public bool IsEnabled { get; set; }
    }
}
