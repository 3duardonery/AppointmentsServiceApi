namespace AppointmentService.Shared.Settings
{
    public sealed class AppSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string FirebaseToken { get; set; }
        public string AuthEndpoint { get; set; }
        public string ProjectId { get; set; }
    }
}
