using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ProfessionalServicesServiceImp _professionalServices;

        public ServicesController(ProfessionalServicesServiceImp professionalServices) 
            => _professionalServices = professionalServices;

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var (isSuccess, results, exception) = await _professionalServices.GetAllServices()
                .ConfigureAwait(false);

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
