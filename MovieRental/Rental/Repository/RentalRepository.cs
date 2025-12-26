using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Rental.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly MovieRentalDbContext _context;

        public RentalRepository(MovieRentalDbContext context)
        {
            _context = context;
        }

        public async Task<ER.Rental> AddAsync(ER.Rental rental)
        {
            if (rental.Movie != null)
            {
                _context.Attach(rental.Movie);
            } 

            _context.Rentals.Add(rental);

            await _context.SaveChangesAsync();

            await _context.Entry(rental)
                  .Reference(r => r.Movie)
                  .LoadAsync();

            return rental;
        }

        public async Task<IEnumerable<ER.Rental>> GetByCustomerNameAsync(string customerName)
        {
            return await _context.Rentals
                .Where(r => EF.Functions.Like(r.CustomerName, customerName))
                .Include(r => r.Movie)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
