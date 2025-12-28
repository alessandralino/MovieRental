using MovieRental.PaymentProviders.Service;
using MovieRental.Rental.DTO;
using MovieRental.Rental.Repository;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Rental.Features
{
	public class RentalFeatures : IRentalFeatures
	{
        private readonly IRentalRepository _repository;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<RentalFeatures> _logger;

        public RentalFeatures(
            IRentalRepository repository,
            IPaymentService paymentService,
            ILogger<RentalFeatures> logger)
        {
            _repository = repository;
            _paymentService = paymentService;
            _logger = logger;
        } 

		public async Task<RentalSaveOutput> Save(RentalSaveInput input)
		{ 
            var totalAmount = CalculateRentalPrice(input.DaysRented);

            var paymentResult = await _paymentService.ProcessPaymentAsync(input.PaymentMethod, totalAmount);

            if (!paymentResult.IsSuccess)
            {
                _logger.LogWarning(
                            "Payment failed. Rental will not be created. MovieId: {MovieId}, PaymentMethod: {PaymentMethod}, Error: {Error}",
                            input.MovieId,
                            input.PaymentMethod,
                            paymentResult.ErrorMessage);

                throw new InvalidOperationException(
                    $"Payment failed: {paymentResult.ErrorMessage}");
            }

            try 
            {
                var rentalEntity = new ER.Rental
                {
                    Movie = input.Movie,
                    DaysRented = input.DaysRented,
                    PaymentMethod = input.PaymentMethod.ToUpper(),
                    CustomerId = input.CustomerId,
                    MovieId = input.MovieId, 
                };

                var savedRental = await _repository.SaveAsync(rentalEntity);

                return new RentalSaveOutput
                {
                    Id = savedRental.Id,
                    Movie = savedRental.Movie,
                    DaysRented = savedRental.DaysRented, 
                    Customer = savedRental.Customer!, 
                    PaymentDetails = new PaymentAmountRentalSaveOutput
                    {
                        PaymentMethod = savedRental.PaymentMethod,
                        PaymentAmount = totalAmount,
                        TransactionId = paymentResult.TransactionId!,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error saving rental to database after successful payment. TransactionId: {TransactionId}, MovieId: {MovieId}, CustomerId: {CustomerId}",
                   paymentResult.TransactionId,
                   input.MovieId,
                   input.CustomerId);

                // TODO: Implement a compensation mechanism here (refund)

                throw new InvalidOperationException(
                    "Error saving rental to database after successful payment. Please contact support.",
                    ex);
            } 
        }
		
		public async Task<IEnumerable<RentalGetByCustomerNameOutput>> GetRentalsByCustomerName(string customerName)
		{
            var rentals = await _repository.GetByCustomerNameAsync(customerName);

            return rentals.Select(r => new RentalGetByCustomerNameOutput
            {
                Id = r.Id,
                Movie = r.Movie,
                DaysRented = r.DaysRented,
                PaymentMethod = r.PaymentMethod,
                Customer = r.Customer!,         
            });
        }

        private static decimal CalculateRentalPrice(int daysRented)
        {
            var valuePerDay = 2.50m;
            return daysRented * valuePerDay;
        }
    }
}
