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
    public sealed class ProfessionalService : ProfessionalServiceImp
    {
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly FactoryProfessionalServicesImp _factoryProfessionalServices;
        private readonly IMapper _mapper;

        public ProfessionalService(FactoryProfessionalImp factoryProfessionalService, FactoryProfessionalServicesImp factoryProfessionalServices, IMapper mapper)
        {
            _factoryProfessional = factoryProfessionalService;
            _factoryProfessionalServices = factoryProfessionalServices;
            _mapper = mapper;
        }

        public async Task<Result<ProfessionalViewModel>> CreateNewProfessional(ProfessionalDto professionalDto)
        {
            var professional = _mapper.Map<Professional>(professionalDto);

            var (isSuccess, result, exception) = await _factoryProfessional.Save(professional).ConfigureAwait(false);

            if (!isSuccess)
                return Result.Error<ProfessionalViewModel>(exception);

            return Result.Success(_mapper.Map<ProfessionalViewModel>(result));
        }

        public async Task<Result<IEnumerable<ProfessionalViewModel>>> GetAllProfessionals()
        {
            var (isSuccess, professionals, exception) = await _factoryProfessional.Professionals().ConfigureAwait(false);

            if (!isSuccess)
                return Result.Error<IEnumerable<ProfessionalViewModel>>(exception);

            return Result.Success(_mapper.Map<IEnumerable<ProfessionalViewModel>>(professionals));
        }

        public async Task<Result> SetServices(string professionalId, List<string> servicesIds)
        {
            var (isSuccess, resultsFound, excpetion) = await _factoryProfessionalServices
                .GetServicesByIds(servicesIds)
                .ConfigureAwait(false);

            if (!isSuccess)
                return excpetion;

            var setServiceOperationResult = 
                await _factoryProfessional.SetServices(professionalId, resultsFound);

            if (!setServiceOperationResult.IsSuccess)
                return setServiceOperationResult.Exception;

            return setServiceOperationResult;
        }
    }
}
