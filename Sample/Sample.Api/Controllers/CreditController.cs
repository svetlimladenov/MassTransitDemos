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
    public class CreditController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public CreditController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Post(int creditId)
        {
            await this.publishEndpoint.Publish<RecalculateCreditMessage>(new { CreditId = creditId });

            return Accepted();
        }
    }
}
