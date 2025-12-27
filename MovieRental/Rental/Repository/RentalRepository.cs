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

        public async Task<ER.Rental> SaveAsync(ER.Rental rental)
        {
            if (rental.Movie != null)
            {
                _context.Attach(rental.Movie);
            }

            if (rental.Customer != null)
            {
                _context.Attach(rental.Customer);
            }

            _context.Rentals.Add(rental);

            await _context.SaveChangesAsync();

            await _context.Entry(rental)
                  .Reference(r => r.Movie) 
                  .LoadAsync();

            await _context.Entry(rental)
                .Reference(r => r.Customer)
                .LoadAsync();

            return rental;
        }

        public async Task<IEnumerable<ER.Rental>> GetByCustomerNameAsync(string customerName)
        {
            return await _context.Rentals
                .Where(r => EF.Functions.Like(r.Customer!.Name, $"%{customerName}%"))
                .Include(r => r.Movie)
                .Include(r => r.Customer)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
