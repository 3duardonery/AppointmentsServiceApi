using System.Collections.Generic;
using System.Linq;
using AppointmentService.Domain.Commands;
using AppointmentService.Domain.Models;
using AppointmentService.Shared.ViewModels;

namespace AppointmentService.Domain.Extensions
{
    public static class ModelExtension
    {
        public static Professional ToEntity(this CreateNewProfessionalCommand professionalDto)
        {
            return new Professional
            {
                Name = professionalDto.Name,
                Email = professionalDto.Email,
                ProfilePicture = professionalDto.ProfilePicture,
                IsEnabled = professionalDto.IsEnabled
            };
        }

        public static ProfessionalViewModel ToViewModel(this Professional professional)
        {
            return new ProfessionalViewModel
            {
                Id = professional.Id,
                Name = professional.Name,
                Email = professional.Email,
                ProfilePicture = professional.ProfilePicture,
                IsEnabled = professional.IsEnabled,
                Services = professional.Services.ToListViewModel()
            };
        }

        public static IEnumerable<ProfessionalViewModel> ToViewModel(this IEnumerable<Professional> professional)
        {
            return professional.Select(x => new ProfessionalViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                ProfilePicture = x.ProfilePicture,
                IsEnabled = x.IsEnabled,
                Services = x.Services.ToListViewModel()
            });
        }

        public static IEnumerable<ServiceViewModel> ToListViewModel(this IEnumerable<Service> services)
        {
            if (services == null) return Enumerable.Empty<ServiceViewModel>();

            return services.Select(service => new ServiceViewModel
            {
                Id = service.Id,
                Description = service.Description,
                Duration = service.Duration,
                IsEnabled = service.IsEnabled
            });
        }
    }
}
