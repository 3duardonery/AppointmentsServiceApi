using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentService.Domain.Repository;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Commands
{
    public sealed class UpdateProfessionalServicesDependenciesCommandHandler :
        IRequestHandler<UpdateProfessionalServicesDependenciesCommand, Result>
    {
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly FactoryProfessionalServicesImp _factoryProfessionalServices;

        public UpdateProfessionalServicesDependenciesCommandHandler(FactoryProfessionalImp factoryProfessionalService, FactoryProfessionalServicesImp factoryProfessionalServices)
        {
            _factoryProfessional = factoryProfessionalService;
            _factoryProfessionalServices = factoryProfessionalServices;
        }

        public async Task<Result> Handle(UpdateProfessionalServicesDependenciesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _factoryProfessionalServices
                    .GetServicesByIds(request.ServiceIds)
                    .ConfigureAwait(false);

                if (!result.IsSuccess)
                    return Result.Error(result.Exception);

                var setServiceOperationResult =
                    await _factoryProfessional.SetServices(request.ProfessionalId, result.Value);

                if (!setServiceOperationResult.IsSuccess)
                    return setServiceOperationResult.Exception;

                return setServiceOperationResult;
            }
            catch (Exception exc)
            {
                return Result.Error(exc);
            }
        }
    }
}
