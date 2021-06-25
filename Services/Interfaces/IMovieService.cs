using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IMovieService
    {
        Task AddMovieAsync(Movie movie);
        Task<Movie> GetMovieAsync(int id);
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task DeleteMovieAsync(int id);
    }
}
