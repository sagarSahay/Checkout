namespace AcquiringBank.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class BarclaysBankController: ControllerBase
    {
        [HttpPost("process-payment")]
        public ActionResult ProcessPayment([FromBody] BankCardRequest request)
        {
            return Ok(new BankResponse()
            {
                PaymentResponseId = Guid.NewGuid(),
                Message = "SUCCESS"
            });
        }
    }
}