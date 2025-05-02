using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentService.Domain.Extensions;
using AppointmentService.Domain.Repository;
using AppointmentService.Shared.ViewModels;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Commands
{
    public sealed class CreateNewProfessionalCommandHandler
        (FactoryProfessionalImp factoryProfessional) :
        IRequestHandler<CreateNewProfessionalCommand, Result<ProfessionalViewModel>>
    {
        public async Task<Result<ProfessionalViewModel>> Handle(CreateNewProfessionalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var professional = request.ToEntity();

                var (isSuccess, result, exception) = await factoryProfessional.Save(professional).ConfigureAwait(false);

                if (!isSuccess)
                    return Result.Error<ProfessionalViewModel>(exception);

                return Result.Success(result.ToViewModel());
            }
            catch (Exception exc)
            {
                return Result.Error<ProfessionalViewModel>(exc);
            }
        }
    }
}
