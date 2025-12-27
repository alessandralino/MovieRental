using MovieRental.Rental.DTO;
using MovieRental.Rental.Repository;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Rental.Features
{
	public class RentalFeatures : IRentalFeatures
	{
        private readonly IRentalRepository _repository;

        public RentalFeatures(IRentalRepository repository)
        {
            _repository = repository;
        } 

		//TODO: make me async :(
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

		//TODO: finish this method and create an endpoint for it
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
