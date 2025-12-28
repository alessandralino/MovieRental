using MovieRental.Rental.DTO;

namespace MovieRental.Rental.Features;

public interface IRentalFeatures
{
    Task<RentalSaveOutput> Save(RentalSaveInput input);
    Task<IEnumerable<RentalGetByCustomerNameOutput>> GetRentalsByCustomerName(string customerName);
}