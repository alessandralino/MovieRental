namespace MovieRental.PaymentProviders.Entities
{
    public class PaymentResult
    {
        public bool IsSuccess { get; init; }
        public string? TransactionId { get; init; }
        public string? ErrorMessage { get; init; }
        public DateTime ProcessedAt { get; init; } = DateTime.UtcNow;

        public static PaymentResult Success(string transactionId) => new()
        {
            IsSuccess = true,
            TransactionId = transactionId
        };

        public static PaymentResult Failure(string errorMessage) => new()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
