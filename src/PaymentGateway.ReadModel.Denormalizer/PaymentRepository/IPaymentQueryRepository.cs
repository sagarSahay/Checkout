namespace PaymentGateway.ReadModel.Denormalizer.PaymentRepository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DocumentContracts;

    public interface IPaymentQueryRepository
    {
        Task<DocumentBase<PaymentVM>> GetById(string id);
        Task<IEnumerable<PaymentVM>> LoadAll();
    }
}