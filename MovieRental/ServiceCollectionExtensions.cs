using MovieRental.Data;
using MovieRental.Movie;
using MovieRental.Rental;

namespace MovieRental
{
    public static class ServiceCollectionExtensions
    {
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
    }
}
