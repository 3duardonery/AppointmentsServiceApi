﻿namespace AppointmentService.Shared.Dto
{
    public sealed class AuthenticationRequest
    {
        public AuthenticationRequest() => ReturnSecureToken = true;

        public string Email { get; set; }
        public string Password { get; set; }
        public bool ReturnSecureToken { get; set; }
    }
}
