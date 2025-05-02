using System;
using System.Collections.Generic;
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
    public sealed class GetAllProfessionalsQueryHandler :
        IRequestHandler<GetAllProfessionalsQuery, Result<IEnumerable<ProfessionalViewModel>>>
    {
        private readonly FactoryProfessionalImp _factoryProfessional;
        private readonly IMemoryCache _memoryCache;
        private const string PROFESSIONAL_KEY = "professional";

        public GetAllProfessionalsQueryHandler(IMemoryCache memoryCache, FactoryProfessionalImp factoryProfessional)
        {
            _memoryCache = memoryCache;
            _factoryProfessional = factoryProfessional;
        }

        public async Task<Result<IEnumerable<ProfessionalViewModel>>> Handle(GetAllProfessionalsQuery request, CancellationToken cancellationToken)
        {
            if (_memoryCache.TryGetValue(PROFESSIONAL_KEY, out IEnumerable<ProfessionalViewModel>? services))
            {
                return Result.Success(services)!;
            }

            var (isSuccess, professionals, exception) = await _factoryProfessional.Professionals().ConfigureAwait(false);

            if (!isSuccess)
                return Result.Error<IEnumerable<ProfessionalViewModel>>(exception!);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            _memoryCache.Set(PROFESSIONAL_KEY, professionals, memoryCacheEntryOptions);

            if (!isSuccess)
                return Result.Error<IEnumerable<ProfessionalViewModel>>(exception!);

            return Result.Success(professionals!.ToViewModel());
        }
    }
}
