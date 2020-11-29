using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sample.Components;
using Sample.Contracts;
using Sample.Contracts.UtilizeCredit;
using Sample.Api.ViewModels;
using AutoMapper;
using Sample.Contracts.UtilizeCredit.GetCreditUtilizationInfo;

namespace Sample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IClientFactory clientFactory;
        private readonly IMapper mapper;

        public CreditController(IPublishEndpoint publishEndpoint, IClientFactory clientFactory, IMapper mapper)
        {
            this.publishEndpoint = publishEndpoint;
            this.clientFactory = clientFactory;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Post(int creditId)
        {
            await this.publishEndpoint.Publish<RecalculateCreditMessage>(new { CreditId = creditId });

            return Accepted();
        }

        [HttpPost("CreateCredit")]
        public async Task<IActionResult> CreateCredit(UtilizeCreditInputModel inputModel)
        {
            var getInfoClient = this.clientFactory.CreateRequestClient<GetCreditUtilizationInfo>();
            using (var request = getInfoClient.Create(new { inputModel.CreateCredit.ExternalId }))
            {
                var stateResponse = await request.GetResponse<CreditUtilizationInfo>();
                if (stateResponse.Message.State != "Not found")
                {
                    return Ok(stateResponse.Message.State);
                }
            }

            var messageData = new
            {
                CreateCredit = mapper.Map<CreateCreditDTO>(inputModel.CreateCredit),
                BonusPoints = mapper.Map<BonusPointDTO>(inputModel.BonusPoints),
            };

            var requestClient = this.clientFactory.CreateRequestClient<UtilizeCreditRequested>();
            using (var request = requestClient.Create(messageData))
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
