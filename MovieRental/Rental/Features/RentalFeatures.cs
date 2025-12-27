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
            var rentalEntity = new ER.Rental
            {
                Movie = input.Movie,
                DaysRented = input.DaysRented,
                PaymentMethod = input.PaymentMethod,
                CustomerId = input.CustomerId,  
                MovieId = input.MovieId
            };

            var savedRental = await _repository.SaveAsync(rentalEntity);

            return new RentalSaveOutput
            {
                Id = savedRental.Id,
                Movie = savedRental.Movie,
                DaysRented = savedRental.DaysRented,
                PaymentMethod = savedRental.PaymentMethod,
                Customer = savedRental.Customer!
            };
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
                CustomerId = r.Customer!,
            });
        }
	}
}
