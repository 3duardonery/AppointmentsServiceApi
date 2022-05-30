using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly ProfessionalServiceImp _professionalService;

        public ProfessionalController(ProfessionalServiceImp professionalService) 
            => _professionalService = professionalService;

        [HttpGet]
        public async Task<IActionResult> GetAllProfessionals()
        {
            var results = await _professionalService
                .GetAllProfessionals().ConfigureAwait(false);

            return Ok(results.Value);
        }

        [HttpPost]
        public async Task<IActionResult> NewProfessional([FromBody] ProfessionalDto professional)
        {
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
