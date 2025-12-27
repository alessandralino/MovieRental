using Microsoft.AspNetCore.Identity;
using MovieRental.Data;

namespace MovieRental.Movie
{
	public class MovieFeatures : IMovieFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public MovieFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}
		
		public Movie Save(Movie movie)
		{
			_movieRentalDb.Movies.Add(movie);
			_movieRentalDb.SaveChangesAsync();
			return movie;
		}

		// TODO: tell us what is wrong in this method? Forget about the async, what other concerns do you have?
		public IQueryable<Movie> GetAll()
		{
			return _movieRentalDb.Movies;
		}
	}
}
