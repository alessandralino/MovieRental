namespace MovieRental.Rental.DTO
{
    public class PaymentAmountRentalSaveOutput
    {
        public string PaymentMethod { get; set; }
        public decimal PaymentAmount { get; set; }
        public string TransactionId { get; set; }
    }
}
