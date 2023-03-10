using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentBookServiceImp _appointmentService;
        private readonly IHub _sentryHub;

        public AppointmentsController(AppointmentBookServiceImp appointmentService, IHub sentryHub)
        {
            _appointmentService = appointmentService;
            _sentryHub = sentryHub;
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
            var childSpan = _sentryHub.GetSpan()?.StartChild("rescheduler-appointment-customer");
            var (isSuccess, result, exception) = await _appointmentService.Reschedule(request).ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(result);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomerAppointments([FromQuery] string customerId)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-appointments-customer");
            var (isSucces, appointments, exception) = await _appointmentService.GetAppointmentsByCustomerId(customerId).ConfigureAwait(false);

            if (!isSucces)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(appointments);
        }

        [HttpGet("professional")]
        public async Task<IActionResult> GetProfessionalAppointments([FromQuery] string email)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-professional-appointments");
            var (isSucces, appointments, exception) = await _appointmentService.GetAppointmentsByProfessionalId(email).ConfigureAwait(false);

            if (!isSucces)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(appointments);
        }

        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelAppointment([FromQuery] string appointmentId)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("cancel-appointment");
            var (isSuccess, exception) = await _appointmentService.CancelAppointment(appointmentId).ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            return Ok();
        }
    }
}
