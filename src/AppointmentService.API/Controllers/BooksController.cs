using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Threading.Tasks;

namespace AppointmentService.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookServiceImp _bookService;
        private readonly IHub _sentryHub;

        public BooksController(BookServiceImp bookService, IHub sentryHub)
        {
            _bookService = bookService;
            _sentryHub = sentryHub;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookByServiceId([FromQuery] string serviceId)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-book-by-service");
            var (isSuccess, result, exception) = await _bookService
                .GetAvailableBooksByServiceId(serviceId)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(result);
        }

        [HttpGet("professional")]
        public async Task<IActionResult> GetBookByEmail([FromQuery] string email)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("get-book-by-email");
            var (isSuccess, result, exception) = await _bookService
                .GetAvailableBooksByProfessionalEmail(email)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateANewBook([FromBody] OpenBookRequestDto openBookRequest)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("create-new-book");
            var (isSuccess, result, exception) = await _bookService.MountANewBookWithLimitInDays(openBookRequest).ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return Created("", result);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBookById([FromBody] CancelBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("bookId does not be null or empty");

            var childSpan = _sentryHub.GetSpan()?.StartChild("cancel-book");

            var (isSuccess, exception) = await _bookService.CancelBookById(request)
                .ConfigureAwait(false);

            if (!isSuccess)
            {
                childSpan.Finish(exception);
                return BadRequest(exception.Message);
            }

            childSpan.Finish(SpanStatus.Ok);

            return NoContent();
        }
    }
}
