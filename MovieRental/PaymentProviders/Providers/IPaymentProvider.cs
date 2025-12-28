using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public interface IPaymentProvider
    {
        string PaymentMethodName { get; }
        Task<PaymentResult> Pay(decimal amount);
    }
}
