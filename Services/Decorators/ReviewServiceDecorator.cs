using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Decorators
{
    public class ReviewServiceDecorator : IReviewService
    {
        private readonly IDbContextFactory<MoviesDBContext> dbContextFactory;

        public ReviewServiceDecorator(IDbContextFactory<MoviesDBContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Result<Review>> AddReviewAsync(int movieId, int reviewerId, Review review)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.AddReviewAsync(movieId, reviewerId, review);
            }
        }

        public async Task<Result<Reviewer>> AddReviewerAsync(Reviewer reviewer)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.AddReviewerAsync(reviewer);
            }
        }

        public Task<Result> DeleteReviewAsync(int reviewerId, int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteReviewerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<IEnumerable<Review>>> GetAllReviewsAsync()
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetAllReviewsAsync();
            }
        }

        public async Task<Result<IEnumerable<Review>>> GetMovieReviewsAsync(int movieId)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetMovieReviewsAsync(movieId);
            }
        }

        public async Task<Result<Review>> GetReviewAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetReviewAsync(id);
            }
        }

        public async Task<Result<Reviewer>> GetReviewerAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetReviewerAsync(id);
            }
        }

        public async Task<Result<IEnumerable<Review>>> GetReviewerReviewsAsync(int reviewerId)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetReviewerReviewsAsync(reviewerId);
            }
        }

        public async Task<Result<Review>> UpdateReviewAsync(int reviewId, int reviewerId, Review review)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.UpdateReviewAsync(reviewerId, reviewId, review);
            }
        }

        public Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer)
        {
            throw new NotImplementedException();
        }
    }
}
