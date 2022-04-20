using AppointmentService.Domain.Models;
using AppointmentService.Domain.Repository;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.ViewModels;
using AutoMapper;
using OperationResult;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Application.Services
{
    public sealed class ProfessionalServicesService : ProfessionalServicesServiceImp
    {
        private readonly FactoryProfessionalServicesImp _professionalServices;
        private readonly IMapper _mapper;

        public ProfessionalServicesService(FactoryProfessionalServicesImp professionalServices, 
            IMapper mapper)
        {
            _professionalServices = professionalServices;
            _mapper = mapper;
        }

        public async Task<Result<ProfessionalServiceViewModel>> CreateNewService(ProfessionalServiceDto serviceDto)
        {
            var service = _mapper.Map<Service>(serviceDto);

            var (isSuccess, result, exception) = await _professionalServices.Save(service)
                .ConfigureAwait(false);

            if (!isSuccess)
                return exception;

            return _mapper.Map<ProfessionalServiceViewModel>(result);
        }

        public async Task<Result<IEnumerable<ProfessionalServiceViewModel>>> GetAllServices()
        {
            var (isSuccess, results, exception) = await _professionalServices.Services()
                .ConfigureAwait(false);

            if (!isSuccess)
                return exception;


            return Result.Success(_mapper.Map<IEnumerable<ProfessionalServiceViewModel>>(results));
        }
    }
}
