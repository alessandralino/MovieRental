using ER = MovieRental.Rental.Entities;

namespace MovieRental.Rental.Repository
{
    public interface IRentalRepository
    {
        Task<ER.Rental> SaveAsync(ER.Rental rental);
        Task<IEnumerable<ER.Rental>> GetByCustomerNameAsync(string customerName);
    }
}
