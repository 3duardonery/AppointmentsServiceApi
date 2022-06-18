using System;
using System.Collections.Generic;

namespace AppointmentService.Shared.ViewModels
{
    public sealed class BookViewModel
    {
        public DateTime Date { get; set; }
        public string BookDateStringValue { get; set; }
        public IEnumerable<TimeViewModel> AvailableHours { get; set; }
        public bool IsEnabled { get; set; }
        public List<ServiceBookViewModel> ServiceReferences { get; set; }
        public ProfessionalBookViewModel ProfessionalReference { get; set; }
    }
}
