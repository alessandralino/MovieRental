namespace MovieRental.Movie;

public interface IMovieFeatures
{
	Movie Save(Movie movie);
    IQueryable<Movie> GetAll();
}