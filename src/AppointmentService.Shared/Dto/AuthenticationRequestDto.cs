namespace AppointmentService.Shared.Dto
{
    public sealed class AuthenticationRequestDto
    {
        public AuthenticationRequestDto() => ReturnSecureToken = true;

        public string Email { get; set; }
        public string Password { get; set; }
        public bool ReturnSecureToken { get; set; }
    }
}
