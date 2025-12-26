namespace MovieRental.Rental.DTO
{
    public class RentalGetByCustomerNameOutput
    {
        public Movie.Movie? Movie { get; set; }
        public int DaysRented { get; set; } 
        public string PaymentMethod { get; set; }
        public string CustomerName { get; set; }
    }
}
