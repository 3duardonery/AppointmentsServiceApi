using System;
using System.Threading;
using System.Threading.Tasks;
using AppointmentService.Domain.Extensions;
using AppointmentService.Domain.Repository;
using AppointmentService.Shared.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using NetDevPack.SimpleMediator;
using OperationResult;

namespace AppointmentService.Domain.Queries
{
    public sealed class GetProfessionalByEmailQueryHandler :
        IRequestHandler<GetProfessionalByEmailQuery, Result<ProfessionalViewModel>>
    {
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly IMemoryCache _memoryCache;
        private const string PROFESSIONAL_KEY = "professional";
        private const string PROFESSIONAL_EMAIL_KEY = "professional_email";

        public GetProfessionalByEmailQueryHandler(FactoryProfessionalImp factoryProfessionalService, IMemoryCache memoryCache)
        {
            _factoryProfessional = factoryProfessionalService;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ProfessionalViewModel>> Handle(GetProfessionalByEmailQuery request, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue(PROFESSIONAL_EMAIL_KEY, out ProfessionalViewModel? professional))
            {
                return professional!;
            }

            var results = await _factoryProfessional
                .GetProfessionalByEmail(request.Email).ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            if (!results.IsSuccess)
                return Result.Error<ProfessionalViewModel>(results.Exception);

            _memoryCache.Set(PROFESSIONAL_EMAIL_KEY, results.Value, memoryCacheEntryOptions);

            return results.Value.ToViewModel();
        }
    }
}
