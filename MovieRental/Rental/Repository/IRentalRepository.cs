using MovieRental.Rental.DTO;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Rental.Repository
{
    public interface IRentalRepository
    {
        Task<ER.Rental> AddAsync(ER.Rental rental);
        Task<IEnumerable<ER.Rental>> GetByCustomerNameAsync(string customerName);
    }
}
