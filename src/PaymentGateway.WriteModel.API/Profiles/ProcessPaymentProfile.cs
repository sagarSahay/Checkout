namespace PaymentGateway.WriteModel.API.Profiles
{
    using AutoMapper;
    using Commands;
    using Models;

    public class ProcessPaymentProfile : Profile
    {
        public ProcessPaymentProfile()
        {
            CreateMap<PaymentRequest, ProcessPayment>();
        }
    }
}