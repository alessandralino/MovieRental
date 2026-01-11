using FluentValidation;
using MovieRental.Data;
using MovieRental.Movie;
using MovieRental.PaymentProviders.Providers;
using MovieRental.PaymentProviders.Service;
using MovieRental.Rental.DTO;
using MovieRental.Rental.Features;
using MovieRental.Rental.Repository;
using MovieRental.Rental.Validators;

namespace MovieRental
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMovieRentalDependencies(
           this IServiceCollection services)
        { 
            services.AddDbContextDependencies();
            services.AddFeaturesDependencies();
            services.AddRepositoryDependencies();
            services.AddValidatorDependencies();
            services.AddPaymentDependencies();

            return services;
        }

        public static IServiceCollection AddDbContextDependencies(
          this IServiceCollection services)
        { 
            services.AddEntityFrameworkSqlite()
                    .AddDbContext<MovieRentalDbContext>(); 

            return services;
        }

        public static IServiceCollection AddFeaturesDependencies(
           this IServiceCollection services)
        {  
            services.AddScoped<IRentalFeatures, RentalFeatures>();
            services.AddScoped<IMovieFeatures, MovieFeatures>();

            return services;
        }

        public static IServiceCollection AddRepositoryDependencies(
          this IServiceCollection services)
        {
            services.AddScoped<IRentalRepository, RentalRepository>(); 

            return services;
        }

        public static IServiceCollection AddValidatorDependencies(
        this IServiceCollection services)
        {
            services.AddScoped<IValidator<RentalSaveInput>, RentalSaveInputValidator>(); 

            return services;
        }

        public static IServiceCollection AddPaymentDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPaymentProvider, MbWayProvider>();
            services.AddScoped<IPaymentProvider, PayPalProvider>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
