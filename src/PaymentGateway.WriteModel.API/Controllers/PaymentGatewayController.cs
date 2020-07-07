namespace PaymentGateway.WriteModel.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Commands;
    using MassTransit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly ISendEndpoint sendEndpoint;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public PaymentGatewayController(ISendEndpoint sendEndpoint, IMapper mapper, ILogger<PaymentGatewayController> logger)
        {
            this.sendEndpoint = sendEndpoint;
            this.mapper = mapper;
            this.logger = logger;
        }
        

        /// <summary>
        /// Processes a payment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /make-payment
        ///     {
        ///        "CardNumber":"1234-1234-1234-1234",
        ///        "ExpiryDate":"01/2021",
        ///        "Cvv":"123",
        ///        "Amount": 12,
        ///        "Currency": "$",
        ///        "OrderId":"1234a",
        ///        "MerchantId": "merchant1"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>A payment id</returns>
        /// <response code="202">Returns the payment id</response>
        /// <response code="400">If the request is bad</response>      
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("make-payment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentRequest request)
        {
            logger.Log(LogLevel.Information, $@"Request received , with order id {request.OrderId}");
            if (!ModelState.IsValid)
            {
                logger.Log(LogLevel.Error, $@"Invalid request, with order id {request.OrderId}");
                return BadRequest(ModelState);
            }

            var command = mapper.Map<ProcessPayment>(request);
            command.PaymentId = Guid.NewGuid();
            logger.Log(LogLevel.Information, $@"Command sent, with order id {request.OrderId} and payment id {command.PaymentId}");
            await sendEndpoint.Send(command);

            return Accepted(new PaymentResponse() {PaymentId = command.PaymentId});
        }
    }
}