using MovieRental.PaymentProviders.Entities;

namespace MovieRental.PaymentProviders.Service
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(string paymentMethod, double amount);
        IEnumerable<string> GetAvailablePaymentMethods();
    }
}
