using MoviesDataLayer.Interfaces;
using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
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

        public Task<Actor> GetMovieAsync(int id)
        {
            return _unitOfWork.Actors.GetByIDAsync(id);
        }
    }
}
