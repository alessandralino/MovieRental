using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class PayPalProvider : IPaymentProvider
    {
        private const int ProcessingDelayMs = 150;
        private const int FailureChance = 20;

        private readonly ILogger<PayPalProvider> _logger;
        public string PaymentMethodName => "PayPal";

        public PayPalProvider(ILogger<PayPalProvider> logger)
        {
            _logger = logger;
        }

        public async Task<PaymentResult> Pay(decimal amount)
        {
            try 
            {
                await ProcessPayment();

                if (ShouldFail())
                {
                    _logger.LogWarning(
                                    "Payment provider temporarily unavailable. Provider: {PaymentMethod}, Amount: {Amount}",
                                    PaymentMethodName,
                                    amount);

                    return PaymentResult.Failure($"{PaymentMethodName} account temporarily unavailable");
                }

                var transactionId = GenerateTransactionId();

                return PaymentResult.Success(transactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while processing payment. Provider: {PaymentMethod}, Amount: {Amount}",
                    PaymentMethodName,
                    amount);

                return PaymentResult.Failure($"Internal error in {PaymentMethodName} processing.");
            }            
        }

        private async Task ProcessPayment()
        {
            await Task.Delay(ProcessingDelayMs);
        }

        // The error occurs when the drawn value is 1.
        // There is a 1 in 20 (5%) chance of error.
        private static bool ShouldFail()
        {
           return Random.Shared.Next(1, FailureChance + 1) == 1;
        }

        private static string GenerateTransactionId()
        {
            return $"PP_{Guid.NewGuid():N}";
        } 
    }
}
