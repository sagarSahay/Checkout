namespace PaymentGateway.ReadModel.API.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        public readonly IServiceProvider serviceProvider;

        public BaseController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}