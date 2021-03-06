using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Decorators
{
    public class MovieServiceDecorator : IMovieService
    {
        private IDbContextFactory<MoviesDBContext> dbContextFactory;

        public MovieServiceDecorator(IDbContextFactory<MoviesDBContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Task AddActorToMovieAsync(int movieId, int actorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Movie>> AddMovieAsync(int producerId, Movie movie)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.AddMovieAsync(producerId, movie);
            }
        }

        public Task DeleteActorFromMovieAsync(int movieId, int actorId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteMovieAsync(int producerId, int movieId)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.DeleteMovieAsync(producerId, movieId);
            }
        }

        public async Task<Result<IEnumerable<Movie>>> GetAllMoviesAsync()
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.GetAllMoviesAsync();
            }
        }

        public Task<IEnumerable<Movie>> GetAllMoviesWithInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Actor>> GetMovieActorsAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Movie>> GetMovieAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.GetMovieAsync(id);
            }
        }

        public async Task<Result<IEnumerable<Movie>>> GetMoviesByProducerIdAsync(int producerId)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.GetMoviesByProducerIdAsync(producerId);
            }
        }

        public Task<Movie> GetMovieWithReviewsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<Movie>> UpdateMovieAsync(int producerId, int movieId, Movie movie)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var movieSerice = new MovieService(unitOfWork);
                return await movieSerice.UpdateMovieAsync(producerId, movieId, movie);
            }
        }
    }
}
