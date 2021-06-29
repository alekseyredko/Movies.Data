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

        public async Task DeleteActorFromMovieAsync(int movieId, int actorId)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithActorsAsync(movieId);
            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found in database with id: {movieId}");
            }

            var actor = movie.Actors.FirstOrDefault(x => x.ActorId == actorId);            
            if (actor == null)
            {
                throw new InvalidOperationException($"Actor not found in movie with id: {movieId}");
            }

            movie.Actors.Remove(actor);
            _unitOfWork.Movies.Update(movie);

            await _unitOfWork.SaveAsync();
        }

        public async Task AddActorToMovieAsync(int movieId, int actorId)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithActorsAsync(movieId);
            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found in database with id: {movieId}");
            }
            
            var actor = await _unitOfWork.Actors.GetByIDAsync(actorId);          
            if (actor == null)
            {
                throw new InvalidOperationException($"Actor not found in movie with id: {actorId}");
            }

            movie.Actors.Add(actor);
            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.SaveAsync();
        }

        public Task<IEnumerable<Movie>> GetAllMoviesWithInfoAsync()
        {
            return _unitOfWork.Movies.GetMoviesWithAllAsync();
        }

        public async Task<IEnumerable<Actor>> GetMovieActorsAsync(int movieId)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithActorsAsync(movieId);

            return movie.Actors;
        }

        public Task<Movie> GetMovieAsync(int id)
        {
            return _unitOfWork.Movies.GetByIDAsync(id);
        }       

        public Task<Movie> GetMovieWithReviewsAsync(int id)
        {
            return _unitOfWork.Movies.GetMovieWithReviewsAsync(id);
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            _unitOfWork.Movies.Update(movie);
            await _unitOfWork.SaveAsync();
        }
    }
}
