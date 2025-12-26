namespace MovieRental.Rental.DTO
{
    public class RentalSaveInput
    {
        public Movie.Movie? Movie { get; set; }
        public int DaysRented { get; set; }
        public int MovieId { get; set; }
        public string PaymentMethod { get; set; }
        public int CustomerId { get; set; }
    }
}
