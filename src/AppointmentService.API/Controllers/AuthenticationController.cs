using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServiceImp _authenticationService;

        public AuthenticationController(AuthenticationServiceImp authenticationService) 
            => _authenticationService = authenticationService;

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] AuthenticationRequest request)
        {
            var (isSuccess, result, exception) = await _authenticationService.LogIn(request).ConfigureAwait(false);

            if (!isSuccess)
                return Unauthorized();

            return Ok(result);
        }
    }
}
