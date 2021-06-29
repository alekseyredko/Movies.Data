using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Movies.Data.DataAccess.Repositiories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MoviesDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Movie>> GetMoviesWithActorsAsync()
        {
            var movies = await context.Movies
                .Include(x => x.Actors)
                .ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMoviesWithAllAsync()
        {
            var movies = await context.Movies
                .Include(x => x.Actors)
                .Include(x => x.Producer)
                .Include(x => x.Reviewers)
                .Include(x => x.Reviews)
                .Include(x => x.ReviewerWatchHistories)
                .ToListAsync();

            return movies;
        }

        public async Task<Movie> GetMovieWithActorsAsync(int movieId)
        {
            var movie = await context.Movies.FindAsync(movieId);
            await context.Entry(movie).Collection(x => x.Actors).LoadAsync();

            return movie;
        }

        public async Task<Movie> GetMovieWithAllAsync(int movieId)
        {
            var movie = await context.Movies.FirstAsync(x => x.MovieId == movieId);
            foreach (var item in context.Entry(movie).Collections)
            {
                await item.LoadAsync();
            }
            await context.Entry(movie).Reference(x => x.Producer).LoadAsync();

            return movie;
        }

        public async Task<Movie> GetMovieWithReviewsAsync(int movieId)
        {
            var movie = await context.Movies.FirstAsync(x => x.MovieId == movieId);
            await context.Entry(movie).Collection(x => x.Reviews).LoadAsync();

            return movie;
        }

        public async Task<Movie> GetMovieWithReviewersAsync(int movieId)
        {
            var movie = await context.Movies.FirstAsync(x => x.MovieId == movieId);
            await context.Entry(movie).Collection(x => x.Reviewers).LoadAsync();

            return movie;
        }
    }
}
