namespace AppointmentService.Shared.Settings
{
    public sealed class AppSettings
    {
        public required string ConnectionString { get; set; }
        public required string Database { get; set; }
        public required string FirebaseToken { get; set; }
        public required string AuthEndpoint { get; set; }
        public required string ProjectId { get; set; }
    }
}
