using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentBookServiceImp _appointmentService;

        public AppointmentsController(AppointmentBookServiceImp appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAppointment([FromBody] AppointmentRequestDto appointmentRequest)
        {
            var (isSuccess, result, exception) = await _appointmentService
                .Save(appointmentRequest).ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest(exception.Message);

            return Created("", result);
        }
    }
}
