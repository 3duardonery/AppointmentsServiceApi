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

        [HttpPost("reshedule")]
        public async Task<IActionResult> RescheduleAppointment([FromBody] ResheduleAppointmentDto request)
        {
            var (isSuccess, result, exception) = await _appointmentService.Reschedule(request).ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest(exception.Message);

            return Ok(result);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerAppointments([FromQuery] string customerId)
        {
            var (isSucces, appointments, exception) = await _appointmentService.GetAppointmentsByCustomerId(customerId).ConfigureAwait(false);

            if (!isSucces)
                return BadRequest(exception.Message);

            return Ok(appointments);
        }

        [HttpGet("professional")]
        public async Task<IActionResult> GetProfessionalAppointments([FromQuery] string professionalId)
        {
            var (isSucces, appointments, exception) = await _appointmentService.GetAppointmentsByProfessionalId(professionalId).ConfigureAwait(false);

            if (!isSucces)
                return BadRequest(exception.Message);

            return Ok(appointments);
        }

        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelAppointment([FromQuery] string appointmentId)
        {
            var resultOfAppointmentCancellation = await _appointmentService.CancelAppointment(appointmentId).ConfigureAwait(false);

            if (!resultOfAppointmentCancellation.IsSuccess)
                return BadRequest(resultOfAppointmentCancellation.Exception);

            return Ok();
        }
    }
}
