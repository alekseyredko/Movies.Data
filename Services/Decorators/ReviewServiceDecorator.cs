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

        public async Task<Result> DeleteReviewAsync(int reviewerId, int reviewId)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.DeleteReviewAsync(reviewerId, reviewId);
            }
        }

        public async Task<Result> DeleteReviewerAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.DeleteReviewerAsync(id);
            }
        }

        public async Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync()
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.GetAllReviewersAsync();
            }
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
                return await reviewService.UpdateReviewAsync(reviewId, reviewerId, review);
            }
        }

        public async Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var reviewService = new ReviewService(unitOfWork);
                return await reviewService.UpdateReviewerAsync(reviewer);
            }
        }
    }
}
