using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Providers
{
    public class MbWayProvider : IPaymentProvider
    {
        private const int ProcessingDelayMs = 100;
        private const int FailureChance = 20; // 1 in 20 = 5%

        public string PaymentMethodName => "MBWay"; 

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
                    return PaymentResult.Failure($"'{PaymentMethodName}' account temporarily");
                }
                 
                var transactionId = GenerateTransactionId();

                return PaymentResult.Success(transactionId);
            }
            catch (Exception)
            {
                return PaymentResult.Failure($"Internal error in '{PaymentMethodName}' processing.");
            }
        }

        private static bool ShouldFail()
        {
            return Random.Shared.Next(1, FailureChance + 1) == 1;
        }

        private static string GenerateTransactionId()
        {
            return $"MB_{Guid.NewGuid():N}";
        }
    }
}
