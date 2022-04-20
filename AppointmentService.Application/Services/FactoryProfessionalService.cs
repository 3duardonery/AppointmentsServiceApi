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
    public sealed class FactoryProfessionalService : FactoryProfessionalServiceImp
    {
        private readonly FactoryProfessionalImp _factoryProfessionalService;
        private readonly IMapper _mapper;

        public FactoryProfessionalService(FactoryProfessionalImp factoryProfessionalService, IMapper mapper)
        {
            _factoryProfessionalService = factoryProfessionalService;
            _mapper = mapper;
        }

        public async Task<Result<ProfessionalViewModel>> CreateNewProfessional(ProfessionalDto professionalDto)
        {
            var professional = _mapper.Map<Professional>(professionalDto);

            var (isSuccess, result, exception) = await _factoryProfessionalService.Save(professional).ConfigureAwait(false);

            if (!isSuccess)
                return Result.Error<ProfessionalViewModel>(exception);

            return Result.Success(_mapper.Map<ProfessionalViewModel>(professional));
        }

        public async Task<Result<IEnumerable<ProfessionalViewModel>>> GetAllProfessionals()
        {
            var professionals = await _factoryProfessionalService.Professionals().ConfigureAwait(false);

            return Result.Success(_mapper.Map<IEnumerable<ProfessionalViewModel>>(professionals.Value));
        }
    }
}
