using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace MovieRental.Rental.Entities
{
	public class Rental
	{
		[Key]
		public int Id { get; set; }
		public int DaysRented { get; set; }
		public string PaymentMethod { get; set; }

        public Movie.Movie? Movie { get; set; }

		[ForeignKey("Movie")]
		public int MovieId { get; set; }

        public Customer.Enitities.Customer? Customer { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
    }
}
