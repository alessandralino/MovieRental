namespace MovieRental.Rental.DTO
{
    public class RentalSaveOutput
    {
        public int Id { get; set; }
        public Movie.Movie? Movie { get; set; }
        public int DaysRented { get; set; } 
        public string PaymentMethod { get; set; }
        public Customer.Enitities.Customer Customer { get; set; }
    }
}
