using Movies.Data.Models;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Movies.Data.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddReviewAsync(Review review, int movieId)
        {
            if (review.Rate <= 0 || review.Rate > 10)
            {
                throw new ArgumentException("Rate must be from 1 to 10", nameof(review.Rate));
            }

            var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(movieId);

            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found in database with id: {movieId}");
            }

            movie.Reviews.Add(review);

            RecalculateTotalMovieScore(movie);
            _unitOfWork.Movies.Update(movie);

            await _unitOfWork.SaveAsync();
        }

        public async Task AddReviewerAsync(Reviewer reviewer)
        {
            try
            {
                await _unitOfWork.Reviewers.InsertAsync(reviewer);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException e)
            {
                throw new InvalidOperationException($"Reviewer already exists in database with id: {reviewer.ReviewerId}");
            }
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetReviewWithMovie(id);
            if (review == null)
            {
                throw new InvalidOperationException($"No such review with id: {id}");
            }

            var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(review.MovieId);

            await _unitOfWork.Reviews.DeleteAsync(id);
            movie.Rate = movie.Reviews
                .Where(x => x.ReviewId != id)
                .Sum(x => x.Rate);

            _unitOfWork.Movies.Update(movie);           
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteReviewerAsync(int id)
        {
            var reviewer = await _unitOfWork.Reviewers.GetReviewerWithAllAsync(id);
            if (reviewer == null)
            {
                throw new InvalidOperationException($"Reviewer not found in database with id: {id}");
            }

            await _unitOfWork.Reviewers.DeleteAsync(id);

            //TODO: recalculate total movie score
            //TODO: delete related reviews
            //TODO: Recalculate all total movie score
            //TODO: Define cascade delete in db

            foreach (var reviewerMovie in reviewer.Movies)
            {
                var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(reviewerMovie.MovieId);
                movie.Rate = movie.Reviews
                    .Where(x => x.ReviewerId != reviewer.ReviewerId)
                    .Sum(x => x.Rate);
                _unitOfWork.Movies.Update(movie);
            }            

            await _unitOfWork.SaveAsync();
        }

        protected void RecalculateTotalMovieScore(Movie movie)
        {            
            movie.Rate = movie.Reviews.Select(x => x.Rate).Sum();
                        
        }

        public async Task<Review> GetReviewAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIDAsync(id);
            if (review == null)
            {
                throw new InvalidOperationException($"Review not found in database with id: {id}");
            }

            return review;
        }

        public async Task<Reviewer> GetReviewerAsync(int id)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(id);
            if (reviewer == null)
            {
                throw new InvalidOperationException($"Reviewer not found in database with id: {id}");
            }

            return reviewer;
        }

        public async Task<Reviewer> GetReviewerWithAllAsync(int id)
        {
            var reviewer = await _unitOfWork.Reviewers.GetReviewerWithAllAsync(id);
            if (reviewer == null)
            {
                throw new InvalidOperationException($"Reviewer not found in database with id: {id}");
            }

            return reviewer;
        }

        public Task<IEnumerable<Reviewer>> GetAllReviewersAsync()
        {
            return _unitOfWork.Reviewers.GetAllAsync();
        }

        public Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return _unitOfWork.Reviews.GetAllAsync();
        }

        public async Task UpdateReviewerAsync(Reviewer reviewer)
        {
            _unitOfWork.Reviewers.Update(reviewer);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateReviewService(Review review)
        {
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.SaveAsync();
        }
    }
}
