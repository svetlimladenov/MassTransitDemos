using Library.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Books.Service.Consumers
{
    public class BookReservedConsumer : IConsumer<BookReserved>
    {
        private readonly ILogger<BookReservedConsumer> logger;

        public BookReservedConsumer(ILogger<BookReservedConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<BookReserved> context)
        {
            logger.LogInformation("Its fucking working again !");
        }
    }
}
