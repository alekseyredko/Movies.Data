using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IMovieService
    {
        Task AddActorToMovieAsync(int movieId, int actorId);
        Task DeleteActorFromMovieAsync(int movieId, int actorId);
        Task<IEnumerable<Actor>> GetMovieActorsAsync(int movieId);
        Task<IEnumerable<Movie>> GetAllMoviesWithInfoAsync();
        Task<Movie> GetMovieWithReviewsAsync(int id);
        

        Task<Result<Movie>> AddMovieAsync(int producerId, Movie movie);
        Task<Result<Movie>> GetMovieAsync(int id);
        Task<Result<IEnumerable<Movie>>> GetAllMoviesAsync();
        Task<Result<IEnumerable<Movie>>> GetMoviesByProducerIdAsync(int producerId);
        Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie movie);
        Task<Result> DeleteMovieAsync(int producerId, int movieId);
    }
}
