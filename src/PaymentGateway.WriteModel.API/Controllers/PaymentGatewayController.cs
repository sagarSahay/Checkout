namespace PaymentGateway.WriteModel.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Commands;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly ISendEndpoint sendEndpoint;
        private readonly IMapper mapper;

        public PaymentGatewayController(ISendEndpoint sendEndpoint, IMapper mapper)
        {
            this.sendEndpoint = sendEndpoint;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("make-payment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = mapper.Map<ProcessPayment>(request);
            command.PaymentId = Guid.NewGuid();
            await sendEndpoint.Send(command);

            return Accepted(new PaymentResponse() {PaymentId = command.PaymentId});
        }

        [HttpGet]
        [Route("hello")]
        public string Hello()
        {
            return "hello";
        }
    }
}