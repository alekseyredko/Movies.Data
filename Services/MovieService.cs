using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _unitOfWork.Movies.InsertAsync(movie);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            await _unitOfWork.Movies.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return movies;
        }

        public Task<Movie> GetMovieAsync(int id)
        {
            return _unitOfWork.Movies.GetByIDAsync(id);
        }
    }
}
