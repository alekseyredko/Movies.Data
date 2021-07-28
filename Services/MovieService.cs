using Movies.Data.Services.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.Data.Results;
using Movies.Data.DataAccess.Interfaces;

namespace Movies.Data.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork _unitOfWork;       

        public MovieService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Movie>> AddMovieAsync(int producerId, Movie movie)
        {
            var result = new Result<Movie>();
            await ResultHandler.TryExecuteAsync(result, AddMovieAsync(producerId, movie,result));
            return result;
        }

        protected async Task<Result<Movie>> AddMovieAsync(int producerId, Movie request, Result<Movie> result)
        {
            var getMovie = await _unitOfWork.Movies.GetMovieByNameAsync(request.MovieName);

            if (getMovie!=null)
            {
                ResultHandler.SetExists(nameof(request.MovieName), result);
                return result;
            }

            request.ProducerId = producerId;

            await _unitOfWork.Movies.InsertAsync(request);
            await _unitOfWork.SaveAsync();

            ResultHandler.SetOk(request, result);
            return result;
        }

        public async Task<Result> DeleteMovieAsync(int producerId, int movieId)
        {
            var result = new Result();
            await ResultHandler.TryExecuteAsync(result, DeleteMovieAsync(producerId, movieId, result));
            return result;
        }

        public async Task<Result> DeleteMovieAsync(int producerId, int movieId, Result result)
        {
            var getMovie = await _unitOfWork.Movies.GetByIDAsync(movieId);

            if (getMovie == null)
            {
                ResultHandler.SetNotFound("MovieId", typeof(Movie), result);
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

            ResultHandler.SetOk(result);

            return result;
        }

        public async Task<Result<IEnumerable<Movie>>> GetAllMoviesAsync()
        {
            var result = new Result<IEnumerable<Movie>>();
            await ResultHandler.TryExecuteAsync(result, GetMoviesAsync(result));

            return result;
        }

        protected async Task<Result<IEnumerable<Movie>>> GetMoviesAsync(Result<IEnumerable<Movie>> result)
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();

            ResultHandler.SetOk(movies, result);

            return result;
        }

        public async Task<Result<IEnumerable<Movie>>> GetMoviesByProducerIdAsync(int producerId)
        {
            var result = new Result<IEnumerable<Movie>>();
            await ResultHandler.TryExecuteAsync(result, GetMoviesByProducerIdAsync(producerId, result));
            return result;
        }

        protected async Task<Result<IEnumerable<Movie>>> GetMoviesByProducerIdAsync(int producerId, Result<IEnumerable<Movie>> result)
        {
            var movies = await _unitOfWork.Movies.GetMoviesByProducerIdAsync(producerId);

            ResultHandler.SetOk(movies, result);

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

        public async Task<Result<Movie>> GetMovieAsync(int id)
        {
            var result = new Result<Movie>();

            await ResultHandler.TryExecuteAsync(result, GetMovieAsync(id, result));

            return result;
        }

        protected async Task<Result<Movie>> GetMovieAsync(int id, Result<Movie> result)
        {
            var movie = await _unitOfWork.Movies.GetByIDAsync(id);
            if (movie == null)
            {
                ResultHandler.SetNotFound(nameof(movie.MovieId), result);
                return result;
            }

            ResultHandler.SetOk(movie, result);

            return result;
        }

        public async Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie movie)
        {
            var result = new Result<Movie>();
            await ResultHandler.TryExecuteAsync(result, UpdateMovieAsync(producerId, movieId, movie, result));
            return result;
        }

        protected async Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie request, Result<Movie> result)
        {
            var getMovie = await _unitOfWork.Movies.GetByIDAsync(movieId);

            if (getMovie == null)
            {
                ResultHandler.SetNotFound(nameof(request.MovieId), result);
                return result;
            }

            if (getMovie.ProducerId != producerId)
            {
                result.ResultType = ResultType.Forbidden;
                result.Title = "Cannot update movie from another producer";
                result.AddError(nameof(getMovie.ProducerId), "Cannot update movie from another producer");
                return result;
            }

            var namesAreNotSame = ResultHandler.CheckStringPropsAreEqual(request.MovieName,
                getMovie.MovieName, nameof(request.MovieName), result);

            if (namesAreNotSame)
            {
                var anotherMovie = await _unitOfWork.Movies.GetMovieByNameAsync(request.MovieName);

                if (anotherMovie != null)
                {
                    ResultHandler.SetExists(nameof(request.MovieName), result);
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

            ResultHandler.SetOk(getMovie, result);

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
