using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> logger;
        private readonly IRequestClient<SubmitOrder> submitOrderRequestClient;
        private readonly IRequestClient<GetOrderInfo> getOrderInfoClieng;
        private readonly IPublishEndpoint publishEndpoint;

        public OrderController(
            ILogger<OrderController> logger,
            IPublishEndpoint publishEndpoint, 
            IRequestClient<SubmitOrder> submitOrderClient,
            IRequestClient<GetOrderInfo> getOrderInfoClieng)
        {
            this.logger = logger;
            this.publishEndpoint = publishEndpoint;
            this.submitOrderRequestClient = submitOrderClient;
            this.getOrderInfoClieng = getOrderInfoClieng;
        }


        // Publish Order
        [HttpPost("PublishOrder")]
        public async Task<IActionResult> PublishOrder(string customerNumber, Guid orderId)
        {
            await this.publishEndpoint.Publish<SubmitOrder>(new
            {
                OrderId = orderId,
                CustomerNumber = customerNumber
            });

            this.logger.LogInformation("Published Submit Order");

            return Ok();
        }

        // Submit Order and wait for response
        [HttpPost("SubmitOrder")]
        public async Task<IActionResult> SubmitOrder(string customerNumber, string orderId)
        {
            // Here we make requst with SubmitOrder message and wait for response
            var(accepted, rejected) = await submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new 
            {
                OrderId = orderId,
                CustomerNumber = customerNumber
            });

            this.logger.LogInformation("Waiting for response");

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;

                return Accepted(response);
            }

            if (accepted.IsCompleted)
            {
                await accepted;

                return Problem("Order was not accepted");
            }
            else
            {
                var response = await rejected;

                return BadRequest(response.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid orderId)
        {
            var response = await getOrderInfoClieng.GetResponse<OrderInfo>(new
            {
                OrderId = orderId
            });

            return Ok(response);
        }
    }
}
