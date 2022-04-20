using System.Collections.Generic;

namespace AppointmentService.Shared.Dto
{
    public sealed class SetServicesRequestDto
    {
        public string ProfessionalId { get; set; }
        public List<string> ServiceIds { get; set; }
    }
}
