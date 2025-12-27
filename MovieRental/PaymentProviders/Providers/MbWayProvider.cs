using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class MbWayProvider : IPaymentProvider
    {
        private const int ProcessingDelayMs = 150;
        private const int FailureChance = 20; // 1 in 20 = 5%

        public string ProviderName => "MBWay"; 

        public async Task<PaymentResult> Pay(decimal amount)
        {
            if (amount <= 0)
            {
                return PaymentResult.Failure("Invalid payment amount.");
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
                return PaymentResult.Failure("Internal error in "+ ProviderName +" processing.");
            }
        }

        private static bool ShouldFail()
        {
            return Random.Shared.Next(1, FailureChance + 1) == 1;
        }

        private static string GenerateTransactionId()
        {
            return $"E_{Guid.NewGuid():N}";
        }
    }
}
