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
                    p => p.PaymentMethodName,
                    p => p, 
                    StringComparer.OrdinalIgnoreCase);
        }   
         
        async Task<PaymentResult> IPaymentService.ProcessPaymentAsync(string paymentMethod, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return PaymentResult.Failure("Payment method is required");
            }

            if (amount <= 0)
            {
                return PaymentResult.Failure("The value must be greater than zero");
            }

            if (!_providers.TryGetValue(paymentMethod, out var provider))
            {
                return PaymentResult.Failure($"Payment method '{paymentMethod}' not supported");
            }

            var result = await provider.Pay(amount);

            return result;
        }
    }
}
