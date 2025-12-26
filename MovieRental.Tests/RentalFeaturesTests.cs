using Microsoft.EntityFrameworkCore;
using Moq;
using MovieRental.Data;
using MovieRental.Rental;

namespace MovieRental.Tests
{
    public class RentalFeaturesTests
    {
        [Fact]
        public async Task Save_Should_Call_AddAndSaveChangesAsync()
        {
            // Arrange
            var rental = new MovieRental.Rental.Rental
            {
                Id = 1,
                CustomerName = "Alice"
            };
             
            var mockSet = new Mock<DbSet<MovieRental.Rental.Rental>>();
            mockSet.Setup(m => m.Add(It.IsAny<MovieRental.Rental.Rental>()));
             
            var mockContext = new Mock<MovieRentalDbContext>();
            mockContext.Setup(c => c.Rentals).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1);

            var rentalFeatures = new RentalFeatures(mockContext.Object);

            // Act
            var result = await rentalFeatures.Save(rental);

            // Assert
            mockSet.Verify(m => m.Add(It.Is<MovieRental.Rental.Rental>(r => r == rental)), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(rental, result);
        }
    }
}
