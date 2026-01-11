using Moq;
using MovieRental.Rental.DTO;
using MovieRental.Rental.Features;
using MovieRental.Rental.Repository;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Tests
{ 
    public class RentalFeaturesTests
    {
        [Fact]
        public async Task Save_Should_CallRepositoryAndReturnOutput()
        {
            // Arrange
            var mockRepo = new Mock<IRentalRepository>();

            var input = new RentalSaveInput
            {
                MovieId = 1,
                DaysRented = 3,
                PaymentMethod = "card",
                CustomerId = 1,
                Movie = new Movie.Movie { Id = 1, Title = "Inception" }
            };
 
            mockRepo.Setup(r => r.SaveAsync(It.IsAny<ER.Rental>()))
                    .ReturnsAsync((ER.Rental r) =>
                    {
                        r.Movie = r.Movie ?? new Movie.Movie 
                        { 
                            Id = r.MovieId, Title = "Inception" 
                        };
                        r.Customer = new Customer.Enitities.Customer
                        { 
                            Id = r.CustomerId, Name = "Alice" 
                        };

                        return r;
                    });

            var features = new RentalFeatures(mockRepo.Object);

            // Act
            var result = await features.Save(input);

            // Assert
            mockRepo.Verify(r => r.SaveAsync(It.Is<ER.Rental>(
                r => r.MovieId == input.MovieId && r.CustomerId == input.CustomerId)), Times.Once);

            Assert.Equal("Alice", result.Customer.Name);         
            Assert.Equal(input.DaysRented, result.DaysRented);
            Assert.Equal(input.PaymentMethod, result.PaymentMethod);
            Assert.Equal("Inception", result.Movie!.Title); 
        }

        [Fact]
        public async Task GetRentalsByCustomerName_Should_ReturnRentals()
        {
            // Arrange
            var mockRepo = new Mock<IRentalRepository>();

            var customerName = "Alice";

            var rentalsFromRepo = new List<ER.Rental>
            {
                new ER.Rental
                {
                    MovieId = 1,
                    DaysRented = 3,
                    CustomerId = 1,
                    PaymentMethod = "card",
                    Movie = new Movie.Movie { Id = 1, Title = "Inception" },
                    Customer = new Customer.Enitities.Customer { Id = 1, Name = "Alice" }  
                },
                new ER.Rental
                {
                    MovieId = 2,
                    DaysRented = 2,
                    CustomerId = 1,
                    PaymentMethod = "cash",
                    Movie = new Movie.Movie { Id = 2, Title = "Titanic" },
                    Customer = new Customer.Enitities.Customer { Id = 1, Name = "Alice" }  
                }
            };

            mockRepo.Setup(r => r.GetByCustomerNameAsync(customerName))
                    .ReturnsAsync(rentalsFromRepo);

            var features = new RentalFeatures(mockRepo.Object);

            // Act
            var result = await features.GetRentalsByCustomerName(customerName);

            // Assert
            mockRepo.Verify(r => r.GetByCustomerNameAsync(customerName), Times.Once);

            Assert.Equal(rentalsFromRepo.Count, result.Count());
             
            foreach (var rental in result)
            {
                var original = rentalsFromRepo.First(r => r.MovieId == rental.Movie!.Id);
                Assert.NotNull(original.Customer);
                Assert.Equal(original.Customer.Name, customerName);
                Assert.Equal(original.DaysRented, rental.DaysRented);
                Assert.Equal(original.PaymentMethod, rental.PaymentMethod);
                Assert.Equal(original.Movie, rental.Movie);
            }
        }
    }
}
