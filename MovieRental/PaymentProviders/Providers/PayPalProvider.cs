using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class PayPalProvider : IPaymentProvider
    {
        private const int ProcessingDelayMs = 150;
        private const int FailureChance = 20; // 1 in 20 = 5%

        public string ProviderName => "PayPal";

        public async Task<PaymentResult> Pay(decimal amount)
        {
            if (amount <= 0)
            {
                return PaymentResult.Failure("Valor do pagamento inválido");
            }

            try 
            {
                await Task.Delay(ProcessingDelayMs);

                if (ShouldFail())
                {
                    return PaymentResult.Failure(ProviderName + " account temporarily unavailable.");
                }

                var transactionId = GenerateTransactionId();

                return PaymentResult.Success(transactionId);
            }
            catch (Exception)
            {
                return PaymentResult.Failure("Internal error in " + ProviderName + " processing.");
            }            
        }

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
