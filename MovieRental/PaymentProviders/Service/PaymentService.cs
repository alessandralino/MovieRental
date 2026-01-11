using MovieRental.PaymentProviders.Entities;
using MovieRental.PaymentProviders.Providers;

namespace MovieRental.PaymentProviders.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly Dictionary<string, IPaymentProvider> _providers;
        private readonly ILogger<PaymentService>? _logger;

        public PaymentService(IEnumerable<IPaymentProvider> providers, ILogger<PaymentService> logger)
        {
            _logger = logger;

            _providers = providers.ToDictionary(
                    p => p.PaymentMethodName,
                    p => p, 
                    StringComparer.OrdinalIgnoreCase);
        }   
         
        async Task<PaymentResult> IPaymentService.ProcessPaymentAsync(string paymentMethod, decimal amount)
        {
            if (!_providers.TryGetValue(paymentMethod, out var provider))
            {
                _logger.LogError(
                    "Payment method '{PaymentMethod}' not supported", 
                    paymentMethod);

                return PaymentResult.Failure($"Payment method '{paymentMethod}' not supported");
            }

            var result = await provider.Pay(amount);

            if (!result.IsSuccess)
            {
                _logger.LogWarning(
                            "Payment processing failed. Method: {PaymentMethod}, Amount: {Amount}, Error: {ErrorMessage}",
                            paymentMethod,
                            amount,
                            result.ErrorMessage);
            }

            return result;
        }
    }
}
