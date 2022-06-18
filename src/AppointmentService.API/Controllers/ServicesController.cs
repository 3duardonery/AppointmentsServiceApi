using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ProfessionalServicesServiceImp _professionalServices;

        private readonly IMemoryCache _memoryCache;
        private const string SERVICES_KEY = "services";

        public ServicesController(ProfessionalServicesServiceImp professionalServices, IMemoryCache memoryCache)
        {
            _professionalServices = professionalServices;
            _memoryCache = memoryCache;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {

            if (_memoryCache.TryGetValue(SERVICES_KEY, out object services))
            {
                return Ok(services);
            }

            var (isSuccess, results, exception) = await _professionalServices.GetAllServices()
                .ConfigureAwait(false);

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200)
            };

            _memoryCache.Set(SERVICES_KEY, results, memoryCacheEntryOptions);

            if (!isSuccess)
                return BadRequest(exception);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewService([FromBody] ProfessionalServiceDto service)
        {
            var (isSuccess, results, exception) = await _professionalServices.CreateNewService(service)
                .ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest(exception);

            return Created("", results);
        }
    }
}
