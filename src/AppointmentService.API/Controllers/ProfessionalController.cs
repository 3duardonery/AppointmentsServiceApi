using System.Threading.Tasks;
using AppointmentService.Domain.Commands;
using AppointmentService.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDevPack.SimpleMediator;

namespace AppointmentService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionalController(IMediator _mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllProfessionals()
        {
            var results = await _mediator.Send(new GetAllProfessionalsQuery())
                 .ConfigureAwait(false);

            if (!results.IsSuccess)
                return BadRequest(results.Exception.Message);

            return Ok(results.Value);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetAllProfessionalByEmail([FromQuery] string email)
        {
            var professional = await _mediator.Send(new GetProfessionalByEmailQuery { Email = email })
                .ConfigureAwait(false);

            if (!professional.IsSuccess)
                return BadRequest(professional.Exception.Message);

            return Ok(professional.Value);
        }

        [HttpPost]
        public async Task<IActionResult> NewProfessional([FromBody] CreateNewProfessionalCommand professional)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var (isSuccess, data, exception) = await _mediator.Send(professional)
                .ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest(exception?.Message);

            return Created("", data);
        }

        [HttpPatch]
        public async Task<IActionResult> AddServiceDependency([FromBody] UpdateProfessionalServicesDependenciesCommand request)
        {

            var result = await _mediator.Send(request)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return BadRequest(result.Exception);

            return Ok();
        }
    }
}
