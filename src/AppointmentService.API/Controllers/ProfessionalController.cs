using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private const string PROFESSIONAL_KEY = "professional";
        private const string PROFESSIONAL_EMAIL_KEY = "professional_email";

        public ProfessionalController(ProfessionalServiceImp professionalService, IMemoryCache memoryCache)
        {
            _professionalService = professionalService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessionals()
        {
            if (_memoryCache.TryGetValue(PROFESSIONAL_KEY, out object services))
            {
                return Ok(services);
            }

            var results = await _professionalService
                .GetAllProfessionals().ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            _memoryCache.Set(PROFESSIONAL_KEY, results, memoryCacheEntryOptions);

            if (!results.IsSuccess)
                return BadRequest(results.Exception.Message);

            return Ok(results);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetAllProfessionalByEmail([FromQuery] string email)
        {
            if (_memoryCache.TryGetValue(PROFESSIONAL_EMAIL_KEY, out object professional))
            {
                return Ok(professional);
            }

            var results = await _professionalService
                .GetAllProfessionalByEmail(email).ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            _memoryCache.Set(PROFESSIONAL_EMAIL_KEY, results.Value, memoryCacheEntryOptions);

            if (!results.IsSuccess)
                return BadRequest(results.Exception.Message);

            return Ok(results.Value);
        }

        [HttpPost]
        public async Task<IActionResult> NewProfessional([FromBody] ProfessionalDto professional)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var (isSuccess, result, excepetion) = await _professionalService.CreateNewProfessional(professional)
                .ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest(excepetion.Message);

            return Created("", result);
        }

        [HttpPatch]
        public async Task<IActionResult> AddServiceDependency([FromBody] SetServicesRequestDto request)
        {
            var result = await _professionalService
                .SetServices(request.ProfessionalId, request.ServiceIds)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return BadRequest(result.Exception);

            return Ok();
        }
    }
}
