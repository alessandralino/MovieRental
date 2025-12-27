using MovieRental.PaymentProviders.Entities;
using MovieRental.PaymentProviders.Providers;

namespace MovieRental.PaymentProviders.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly Dictionary<string, IPaymentProvider> _providers;
        public PaymentService(IEnumerable<IPaymentProvider> providers)
        {
            _providers = providers.ToDictionary(
                    p => p.ProviderName,
                    p => p, 
                    StringComparer.OrdinalIgnoreCase);
        }   

        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, double amount)
        {
            throw new NotImplementedException();
        }
    }
}
