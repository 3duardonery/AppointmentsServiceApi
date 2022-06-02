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
                return BadRequest(exception.Message);

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

        [HttpDelete]
        public async Task<IActionResult> CancelBookById([FromRoute] string bookId)
        {
            if (string.IsNullOrEmpty(bookId))
                return BadRequest("bookId does not be null or empty");

            var result = await _bookService.CancelBookById(bookId)
                .ConfigureAwait(false);

            if (!result.IsSuccess)
                return BadRequest(result.Exception.Message);

            return NoContent();
        }
    }
}
