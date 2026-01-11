using FluentValidation;

namespace MovieRental.Rental.Validators
{
    public class CustomerNameValidator : AbstractValidator<string>
    {
        public CustomerNameValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("Customer name is required.")
                .MinimumLength(2)
                .MaximumLength(100)
                .Matches("^[a-zA-ZÀ-ÿ\\s]+$")
                .WithMessage("Customer name contains invalid characters.");
        }
    }
}

