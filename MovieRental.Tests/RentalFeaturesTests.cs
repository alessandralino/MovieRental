using Microsoft.Extensions.Logging;
using Moq;
using MovieRental.PaymentProviders.Entities;
using MovieRental.PaymentProviders.Service;
using MovieRental.Rental.DTO;
using MovieRental.Rental.Features;
using MovieRental.Rental.Repository;
using ER = MovieRental.Rental.Entities;

namespace MovieRental.Tests
{ 
    public class RentalFeaturesTests
    {
        Mock<IRentalRepository> _mockRepo;
        Mock<IPaymentService> _mockPayment;
        Mock<ILogger<RentalFeatures>> _mockLogger;

        public RentalFeaturesTests()
        {
            _mockRepo = new Mock<IRentalRepository>();
            _mockPayment = new Mock<IPaymentService>();
            _mockLogger = new Mock<ILogger<RentalFeatures>>();
        }


        [Fact]
        public async Task Save_Should_CallRepositoryAndReturnOutput()
        {
            // Arrange 
            var input = new RentalSaveInput
            {
                MovieId = 1,
                DaysRented = 3,
                PaymentMethod = "mbway",
                CustomerId = 1,
                Movie = new Movie.Movie { Id = 1, Title = "Inception" }
            };
 
            _mockRepo.Setup(r => r.SaveAsync(It.IsAny<ER.Rental>()))
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

            _mockPayment.Setup(s => s.ProcessPaymentAsync(
                        It.IsAny<string>(), 
                        It.IsAny<decimal>()))
                .ReturnsAsync(new PaymentResult
                {
                    IsSuccess = true,
                    TransactionId = "TX123"
                });
             

            var features = new RentalFeatures(_mockRepo.Object, _mockPayment.Object, _mockLogger.Object);

            // Act
            var result = await features.Save(input);

            // Assert
            _mockRepo.Verify(r => r.SaveAsync(It.Is<ER.Rental>(
                r => r.MovieId == input.MovieId && r.CustomerId == input.CustomerId)), Times.Once);

            Assert.Equal("Alice", result.Customer.Name);         
            Assert.Equal(input.DaysRented, result.DaysRented);
            Assert.Equal(input.PaymentMethod.ToUpper(), result.PaymentDetails.PaymentMethod);
            Assert.Equal("Inception", result.Movie!.Title); 
        }

        [Fact]
        public async Task GetRentalsByCustomerName_Should_ReturnRentals()
        {
            // Arrange
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

            _mockRepo.Setup(r => r.GetByCustomerNameAsync(customerName))
                    .ReturnsAsync(rentalsFromRepo);

            var features = new RentalFeatures(_mockRepo.Object, _mockPayment.Object, _mockLogger.Object);

            // Act
            var result = await features.GetRentalsByCustomerName(customerName);

            // Assert
            _mockRepo.Verify(r => r.GetByCustomerNameAsync(customerName), Times.Once);

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
