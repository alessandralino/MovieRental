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

        public RentalFeatures(
            IRentalRepository repository,
            IPaymentService paymentService)
        {
            _repository = repository;
            _paymentService = paymentService;
        } 

		public async Task<RentalSaveOutput> Save(RentalSaveInput input)
		{ 
            var totalAmount = CalculateRentalPrice(input.DaysRented);

            var paymentResult = await _paymentService.ProcessPaymentAsync(input.PaymentMethod, totalAmount);

            if (!paymentResult.IsSuccess)
            { 
                throw new InvalidOperationException($"Payment failed: {paymentResult.ErrorMessage}");
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
                //TODO: Create rollback payment logic here
                //TODO: Create payment table to be able to track payments and do rollbacks
                throw new InvalidOperationException(
                    $"Error saving rental to database after successful payment. Please contact support.", ex);
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
