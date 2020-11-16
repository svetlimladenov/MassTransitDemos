using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Contracts;

namespace Sample_Youtube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GosheController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public GosheController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> SendMessageToGoshe(string message)
        {
            await this.publishEndpoint.Publish<MessageToGoshe>(new 
            { 
                Message = message
            });

            return Accepted();
        }
    }
}
