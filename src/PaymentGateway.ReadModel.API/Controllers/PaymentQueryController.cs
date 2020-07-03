namespace PaymentGateway.ReadModel.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Denormalizer.PaymentRepository;
    using Microsoft.AspNetCore.Mvc;

    [Route("api")]
    public class PaymentQueryController : BaseController
    {
        private readonly IPaymentQueryRepository paymentQueryRepository;
        
        public PaymentQueryController(IServiceProvider serviceProvider, IPaymentQueryRepository paymentQueryRepository) : base(serviceProvider)
        {
            this.paymentQueryRepository = paymentQueryRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var paymentObj = await paymentQueryRepository.GetById(id);
            if (paymentObj == null) return NotFound();
            return Ok(paymentObj.VM);
        }
    }
}