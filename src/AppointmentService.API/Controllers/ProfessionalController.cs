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
    public class ProfessionalController : ControllerBase
    {
        private readonly ProfessionalServiceImp _professionalService;
        private readonly IMemoryCache _memoryCache;
        private readonly IHub _sentryHub;
        private const string PROFESSIONAL_KEY = "professional";
        private const string PROFESSIONAL_EMAIL_KEY = "professional_email";
        private const string CACHE_DESCRIPTION = "CACHED_VALUES";
        private const int ONE_HOUR_IN_SECONDS = 3600;
        private const int TWENTY_MINUTES_IN_SECONDS = 1200;

        public ProfessionalController(ProfessionalServiceImp professionalService, IMemoryCache memoryCache, IHub sentryHub)
        {
            _professionalService = professionalService;
            _memoryCache = memoryCache;
            _sentryHub = sentryHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessionals()
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-all-professionals");
            if (_memoryCache.TryGetValue(PROFESSIONAL_KEY, out object services))
            {
                childSpan.Description = CACHE_DESCRIPTION;
                childSpan.Finish(SpanStatus.Ok);
                return Ok(services);
            }

            var (isSuccess, professionals, exception) = await _professionalService
                .GetAllProfessionals().ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ONE_HOUR_IN_SECONDS),
                SlidingExpiration = TimeSpan.FromSeconds(TWENTY_MINUTES_IN_SECONDS)
            };

            _memoryCache.Set(PROFESSIONAL_KEY, professionals, memoryCacheEntryOptions);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(professionals);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetAllProfessionalByEmail([FromQuery] string email)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-all-professionals-by-email");
            if (_memoryCache.TryGetValue(PROFESSIONAL_EMAIL_KEY, out object professionalCache))
            {
                return Ok(professionalCache);
            }

            var (isSuccess, professional, exception) = await _professionalService
                .GetAllProfessionalByEmail(email).ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ONE_HOUR_IN_SECONDS),
                SlidingExpiration = TimeSpan.FromSeconds(TWENTY_MINUTES_IN_SECONDS)
            };

            _memoryCache.Set(PROFESSIONAL_EMAIL_KEY, professional, memoryCacheEntryOptions);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(professional);
        }

        [HttpPost]
        public async Task<IActionResult> NewProfessional([FromBody] ProfessionalDto professional)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var childSpan = _sentryHub.GetSpan()?.StartChild("create-new-professional");

            var (isSuccess, result, exception) = await _professionalService.CreateNewProfessional(professional)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Created("", result);
        }

        [HttpPatch]
        public async Task<IActionResult> AddServiceDependency([FromBody] SetServicesRequestDto request)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("add-service-dependency");
            var (isSuccess, exception) = await _professionalService
                .SetServices(request.ProfessionalId, request.ServiceIds)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok();
        }
    }
}
