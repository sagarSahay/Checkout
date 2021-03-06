using PaymentGateway.Events.v1;

namespace PaymentGateway.ReadModel.Denormalizer.Handlers
{
    using System.Threading.Tasks;
    using DbRepositoryContracts;
    using DocumentContracts;
    using MassTransit;
    using PaymentRepository;

    public class PaymentDenormalizer : 
        IConsumer<PaymentSuccessful>, 
        IConsumer<PaymentUnsuccessful>,
        IConsumer<PaymentError>
    {
        private IDenormalizerRepository<PaymentVM> repository;

        public PaymentDenormalizer(IDenormalizerRepository<PaymentVM> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessful> context)
        {
            var message = context.Message;
            var id = message.PaymentId.ToString();
            
            var paymentVm = new PaymentVM()
            {
                PaymentId = message.PaymentId,
                PaymentResponseId = message.PaymentResponseId,
                PaymentResponseStatus = message.PaymentResponseStatus,
                OrderId = message.OrderId,
                Amount = message.Amount,
                CardNumber = message.CardNumber,
                Currency = message.Currency,
                MerchantId = message.MerchantId,
                PaymentStatus = "Successful",
                Cvv = message.Cvv,
                ExpiryDate = message.ExpiryDate
            };
            
            var paymentDoc = new DocumentBase<PaymentVM>(){ VM = paymentVm};

            await repository.Upsert(id, paymentDoc);
        }

        public async Task Consume(ConsumeContext<PaymentUnsuccessful> context)
        {
            var message = context.Message;
            var id = message.PaymentId.ToString();
            
            var paymentVm = new PaymentVM()
            {
                PaymentId = message.PaymentId,
                PaymentResponseId = message.PaymentResponseId,
                PaymentResponseStatus = message.PaymentResponseStatus,
                OrderId = message.OrderId,
                Amount = message.Amount,
                CardNumber = message.CardNumber,
                Currency = message.Currency,
                MerchantId = message.MerchantId,
                PaymentStatus = "Unsuccessful",
                Cvv = message.Cvv,
                ExpiryDate = message.ExpiryDate
            };
            
            var paymentDoc = new DocumentBase<PaymentVM>(){ VM = paymentVm};

            await repository.Upsert(id, paymentDoc);
        }
        
        public async Task Consume(ConsumeContext<PaymentError> context)
        {
            var message = context.Message;
            var id = message.PaymentId.ToString();
            
            var paymentVm = new PaymentVM()
            {
                PaymentId = message.PaymentId,
                OrderId = message.OrderId,
                Amount = message.Amount,
                CardNumber = message.CardNumber,
                Currency = message.Currency,
                MerchantId = message.MerchantId,
                PaymentStatus = "System error",
                Cvv = message.Cvv,
                ExpiryDate = message.ExpiryDate
            };
            
            var paymentDoc = new DocumentBase<PaymentVM>(){ VM = paymentVm};

            await repository.Upsert(id, paymentDoc);
        }
    }
}