using Movies.Data.Models;
using MoviesDataLayer;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IMovieRepository: IGenericRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetMoviesWithAllAsync();
        Task<IEnumerable<Movie>> GetMoviesWithActorsAsync();        
        Task<Movie> GetMovieWithActorsAsync(int movieId);
        Task<Movie> GetMovieWithAllAsync(int movieId);
        Task<Movie> GetMovieWithReviewsAsync(int movieId);
        Task<Movie> GetMovieWithReviewersAsync(int movieId);
        Task<Movie> GetMovieByNameAsync(string movieName);
    }
}