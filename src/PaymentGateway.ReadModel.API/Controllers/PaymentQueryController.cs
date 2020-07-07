namespace PaymentGateway.ReadModel.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Denormalizer.PaymentRepository;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/query")]
    public class PaymentQueryController : BaseController
    {
        private readonly IPaymentQueryRepository paymentQueryRepository;
        
        public PaymentQueryController(IServiceProvider serviceProvider, 
            IPaymentQueryRepository paymentQueryRepository) : base(serviceProvider)
        {
            this.paymentQueryRepository = paymentQueryRepository;
        }

        [HttpGet("/payment-info/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var paymentObj = await paymentQueryRepository.GetById(id);
            if (paymentObj == null) return NotFound();
            return Ok(paymentObj.VM);
        }
        
        [HttpGet("/merchant-info/{id}")]
        public async Task<IActionResult> GetMerchantTransactions(string id)
        {
            var allTransactions = await paymentQueryRepository.LoadAll();
            if (allTransactions == null) return NotFound();
            var merchantTransactions = allTransactions.Where(x => x.MerchantId == id);

            return Ok(merchantTransactions);
        }
    }
}