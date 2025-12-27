using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class PayPalProvider : IPaymentProvider
    {
        public string ProviderName => "PayPal";

        Task<PaymentResult> IPaymentProvider.Pay(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
