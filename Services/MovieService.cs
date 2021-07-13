using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Results;

namespace Movies.Data.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResultHandlerService _resultHandlerService;
        public MovieService(IUnitOfWork unitOfWork, IResultHandlerService resultHandlerService)
        {
            _unitOfWork = unitOfWork;
            _resultHandlerService = resultHandlerService;
        }

        public Task<Result<Movie>> AddMovieAsync(int producerId, Movie movie)
        {
            movie.ProducerId = producerId;
            return _resultHandlerService.HandleTaskAsync(movie, AddMovieAsync);
        }

        protected async Task<Result<Movie>> AddMovieAsync(Movie request, Result<Movie> result)
        {
            var getMovie = await _unitOfWork.Movies.GetMovieByNameAsync(request.MovieName);

            if (getMovie!=null)
            {
                _resultHandlerService.SetExists(nameof(request.MovieName), result);
                return result;
            }

            await _unitOfWork.Movies.InsertAsync(request);
            await _unitOfWork.SaveAsync();

            _resultHandlerService.SetOk(request, result);
            return result;
        }

        public Task<Result> DeleteMovieAsync(int producerId, int movieId)
        {
            return _resultHandlerService.HandleTaskAsync(producerId, movieId, DeleteMovieAsync);
        }

        public async Task<Result> DeleteMovieAsync(int producerId, int movieId, Result result)
        {
            var getMovie = await _unitOfWork.Movies.GetByIDAsync(movieId);

            if (getMovie == null)
            {
                _resultHandlerService.SetNotFound("MovieId", typeof(Movie), result);
                return result;
            }

            if (getMovie.ProducerId != producerId)
            {
                result.ResultType = ResultType.Forbidden;
                result.Title = "Cannot delete movie from another producer";
                result.AddError(nameof(getMovie.ProducerId), "Cannot delete movie from another producer");
                return result;
            }

            await _unitOfWork.Movies.DeleteAsync(movieId);
            await _unitOfWork.SaveAsync();

            _resultHandlerService.SetOk(result);

            return result;
        }

        public Task<Result<IEnumerable<Movie>>> GetAllMoviesAsync()
        {
            return _resultHandlerService.HandleTaskAsync<IEnumerable<Movie>>(GetMoviesAsync);
        }

        protected async Task<Result<IEnumerable<Movie>>> GetMoviesAsync(Result<IEnumerable<Movie>> result)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();

            _resultHandlerService.SetOk(movies, result);

            return result;
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

            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found in database with id: {movieId}");
            }

            return movie.Actors;
        }

        public Task<Result<Movie>> GetMovieAsync(int id)
        {
            return _resultHandlerService.HandleTaskAsync<Movie, int>(id, GetMovieAsync);
        }

        protected async Task<Result<Movie>> GetMovieAsync(int id, Result<Movie> result)
        {
            var movie = await _unitOfWork.Movies.GetByIDAsync(id);
            if (movie == null)
            {
                _resultHandlerService.SetNotFound(nameof(movie.MovieId), result);
                return result;
            }

            _resultHandlerService.SetOk(movie, result);

            return result;
        }

        public Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie movie)
        {
            return _resultHandlerService.HandleTaskAsync(producerId, movieId, movie, UpdateMovieAsync);
        }

        protected async Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie request, Result<Movie> result)
        {
            var getMovie = await _unitOfWork.Movies.GetByIDAsync(movieId);

            if (getMovie == null)
            {
                _resultHandlerService.SetNotFound(nameof(request.MovieId), result);
                return result;
            }

            if (getMovie.ProducerId != producerId)
            {
                result.ResultType = ResultType.Forbidden;
                result.Title = "Cannot update movie from another producer";
                result.AddError(nameof(getMovie.ProducerId), "Cannot update movie from another producer");
                return result;
            }

            var namesAreNotSame = _resultHandlerService.CheckStringPropsAreEqual(request.MovieName,
                getMovie.MovieName, nameof(request.MovieName), result);

            if (namesAreNotSame)
            {
                var anotherMovie = await _unitOfWork.Movies.GetMovieByNameAsync(request.MovieName);

                if (anotherMovie != null)
                {
                    _resultHandlerService.SetExists(nameof(request.MovieName), result);
                }
                else
                {
                    getMovie.MovieName = request.MovieName;
                }
            }

            getMovie.Duration = request.Duration;

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            _resultHandlerService.SetOk(getMovie, result);

            _unitOfWork.Movies.Update(getMovie);
            await _unitOfWork.SaveAsync();

            return result;
        }

        public async Task<Movie> GetMovieWithReviewsAsync(int id)
        {
            var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(id);
            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found in database with id: {id}");
            }

            return movie;
        }
    }
}
