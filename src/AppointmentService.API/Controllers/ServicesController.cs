using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sentry;
using System;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ProfessionalServicesServiceImp _professionalServices;

        private readonly IMemoryCache _memoryCache;
        private readonly IHub _sentryHub;
        private const string SERVICES_KEY = "services";
        private const string CACHE_DESCRIPTION = "CACHED_VALUES";
        private const int ONE_HOUR_IN_SECONDS = 3600;
        private const int TWENTY_MINUTES_IN_SECONDS = 1200;

        public ServicesController(ProfessionalServicesServiceImp professionalServices, IMemoryCache memoryCache, IHub sentryHub)
        {
            _professionalServices = professionalServices;
            _memoryCache = memoryCache;
            _sentryHub = sentryHub;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-all-services");
            if (_memoryCache.TryGetValue(SERVICES_KEY, out object services))
            {
                childSpan.Description = CACHE_DESCRIPTION;
                childSpan.Finish(SpanStatus.Ok);
                return Ok(services);
            }

            var (isSuccess, results, exception) = await _professionalServices.GetAllServices()
                .ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ONE_HOUR_IN_SECONDS),
                SlidingExpiration = TimeSpan.FromSeconds(TWENTY_MINUTES_IN_SECONDS)
            };

            _memoryCache.Set(SERVICES_KEY, results, memoryCacheEntryOptions);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewService([FromBody] ProfessionalServiceDto service)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("create-new-service");
            var (isSuccess, results, exception) = await _professionalServices.CreateNewService(service)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Created("", results);
        }
    }
}
