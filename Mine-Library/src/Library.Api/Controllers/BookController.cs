using Library.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IPublishEndpoint publishEndpoint;

        public BookController(ILogger<BookController> logger,IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(Guid bookId, string isbn, string title)
        {
            await this.publishEndpoint.Publish<BookAdded>(new
            {
                BookId = bookId,
                Timestamp = DateTime.Now,
                Isbn = isbn,
                Title = title
            }); ;

            return Accepted();
        }

        [HttpPost("ReserveBook")]
        public async Task<IActionResult> ReserveBook(Guid bookId)
        {
            await this.publishEndpoint.Publish<ReservationRequested>(new
            {
                BookId = bookId,
                Timestamp = DateTime.Now,
                ReservationId = NewId.NextGuid(),
                MemberId = NewId.NextGuid()
            });

            return Accepted();
        }
    }
}
