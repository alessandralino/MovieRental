using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class MbWayProvider : IPaymentProvider
    {
        public string ProviderName => "MBWay";
         
        Task<PaymentResult> IPaymentProvider.Pay(double amount)
        {
            throw new NotImplementedException();
        }
    }
}
