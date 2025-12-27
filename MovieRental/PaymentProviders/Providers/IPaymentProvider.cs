using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public interface IPaymentProvider
    {
        string ProviderName { get; }
        Task<PaymentResult> Pay(decimal amount);
    }
}
