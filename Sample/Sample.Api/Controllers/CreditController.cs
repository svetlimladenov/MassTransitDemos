using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Components;
using Sample.Contracts;
using Sample.Contracts.UtilizeCredit;
using Sample.Api.ViewModels;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IClientFactory clientFactory;

        public CreditController(IPublishEndpoint publishEndpoint, IClientFactory clientFactory)
        {
            this.publishEndpoint = publishEndpoint;
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> Post(int creditId)
        {
            await this.publishEndpoint.Publish<RecalculateCreditMessage>(new { CreditId = creditId });

            return Accepted();
        }

        [HttpPost("CreateCredit")]
        public async Task<IActionResult> CreateCredit(CreateCreditInputModel inputModel)
        {
            var requestClient = this.clientFactory.CreateRequestClient<UtilizeCreditRequested>();

            var model = new CreateCreditDTO
            {
                ExternalId = inputModel.ExternalId,
                Sum = inputModel.Sum,
                UtilizationDate = inputModel.UtilizationDate,
                FirstPaymentDate =  inputModel.FirstPaymentDate
            };

            using (var request = requestClient.Create(new { CreateCredit = model }))
            {
                var (statusResponse, errorResponse) = await request.GetResponses<UtilizeCreditCompleted, UtilizeCreditFaulted>();

                if (statusResponse.IsCompletedSuccessfully)
                {
                    var data = await statusResponse;
                    return Ok(data);
                }
                else
                {
                    var data = await errorResponse;
                    return Ok(data);
                }
            }
        }
    }
}
