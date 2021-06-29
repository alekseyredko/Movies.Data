using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IMovieService
    {
        Task AddActorToMovieAsync(int movieId, int actorId);
        Task DeleteActorFromMovieAsync(int movieId, int actorId);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task<Movie> GetMovieAsync(int id);        
        Task<IEnumerable<Actor>> GetMovieActorsAsync(int movieId);
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<IEnumerable<Movie>> GetAllMoviesWithInfoAsync();
        Task DeleteMovieAsync(int id);
        Task<Movie> GetMovieWithReviewsAsync(int id);
        //    Task AddReviewAsync(int movieId, Review review);
        //    Task DeleteReviewAsync(int movieId, int reviewId);
        //    Task<Movie> GetMovieWithReviewAsync(int movieId);
        //    Task<IEnumerable<Review>> GetMovieReviews(int movieId);
    }
}
