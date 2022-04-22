using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookServiceImp _bookService;

        public BooksController(BookServiceImp bookService) 
            => _bookService = bookService;

        [HttpGet]
        public async Task<IActionResult> GetBookByServiceId([FromQuery] string serviceId)
        {
            var (isSuccess, result, exception) = await _bookService
                .GetAvailableBooksByServiceId(serviceId)
                .ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateANewBook([FromBody] OpenBookRequestDto openBookRequest)
        {
            var (isSuccess, result, exception) = await _bookService.MountANewBookWithLimitInDays(openBookRequest).ConfigureAwait(false);

            if (!isSuccess)
                return BadRequest();

            return Created("", result);
        }

    }
}
