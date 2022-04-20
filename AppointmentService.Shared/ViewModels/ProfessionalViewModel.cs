using System.Collections.Generic;

namespace AppointmentService.Shared.ViewModels
{
    public sealed class ProfessionalViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public bool IsEnabled { get; set; }
        public IEnumerable<ProfessionalServiceViewModel> Services { get; set; }
    }
}
