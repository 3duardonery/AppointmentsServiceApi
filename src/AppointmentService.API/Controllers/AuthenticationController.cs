using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServiceImp _authenticationService;
        private readonly IHub _sentryHub;

        public AuthenticationController(AuthenticationServiceImp authenticationService, IHub sentryHub)
        {
            _authenticationService = authenticationService;
            _sentryHub = sentryHub;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] AuthenticationRequestDto request)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("authentication-firebase");
            if (!ModelState.IsValid)
                return BadRequest();

            var (isSuccess, result, exception) = await _authenticationService.LogIn(request).ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return Unauthorized();
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(result);
        }
    }
}
