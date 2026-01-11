using FluentValidation;
using MovieRental.Rental.DTO;

namespace MovieRental.Rental.Validators
{
    public class RentalSaveInputValidator : AbstractValidator<RentalSaveInput>
    {
        public RentalSaveInputValidator()
        {
            RuleFor(x => x.MovieId)
                .GreaterThan(0)
                .WithMessage("MovieId must be valid");

            RuleFor(x => x.DaysRented)
                .GreaterThan(0)
                .WithMessage("DaysRented must be greater than zero");

            RuleFor(x => x.CustomerId)
                .NotEmpty();

            RuleFor(x => x.PaymentMethod)
                .NotEmpty();
        }
    }
}
