using Movies.Data.Models;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using Castle.Core.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Results;

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

        public async Task<Result<Reviewer>> AddReviewerAsync(Reviewer reviewer)
        {
            var result = new Result<Reviewer>();
            await ResultHandler.TryExecuteAsync(result, AddReviewerAsync(reviewer, result));
            return result;
        }

        protected async Task<Result<Reviewer>> AddReviewerAsync(Reviewer request, Result<Reviewer> result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(request.ReviewerId);

            if (reviewer != null)
            {
                ResultHandler.SetExists(nameof(request.ReviewerId), result);
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            await _unitOfWork.Reviewers.InsertAsync(request);
            await _unitOfWork.SaveAsync();

            result.ResultType = ResultType.Ok;
            result.Value = request;
            result.Title = "Success!";

            return result;
        }

        public async Task<Result> DeleteReviewerAsync(int id)
        {
            var result = new Result();
            await ResultHandler.TryExecuteAsync(result, DeleteReviewerAsync(id, result));
            return result;
        }

        public async Task<Result> DeleteReviewerAsync(int id, Result result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(id);
            if (reviewer == null)
            {
                ResultHandler.SetAccountNotFound("Id", result);
                return result;
            }

            await _unitOfWork.Reviewers.DeleteAsync(id);

            reviewer = await _unitOfWork.Reviewers.GetFullReviewerAsync(id);

            foreach (var reviewerMovie in reviewer.Movies)
            {
                var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(reviewerMovie.MovieId);
                movie.Reviews = movie.Reviews.Where(x => x.ReviewerId != reviewer.ReviewerId).ToList();

                RecalculateTotalMovieScore(movie);
                _unitOfWork.Movies.Update(movie);
            }

            await _unitOfWork.SaveAsync();

            ResultHandler.SetOk(result);

            return result;
        }

        protected void RecalculateTotalMovieScore(Movie movie)
        {            
            movie.Rate = movie.Reviews.Sum(x => x.Rate);
        }

        public async Task<Result<Reviewer>> GetReviewerAsync(int id)
        {
            var result = new Result<Reviewer>();
            await ResultHandler.TryExecuteAsync(result, GetReviewerAsyncResult(id, result));
            return result;
        }

        protected async Task<Result<Reviewer>> GetReviewerAsyncResult(int id, Result<Reviewer> result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(id);
            if (reviewer == null)
            {
                ResultHandler.SetNotFound(nameof(reviewer.ReviewerId), result);
                return result;
            }

            ResultHandler.SetOk(reviewer, result);

            return result;
        }

        public async Task<Reviewer> GetReviewerWithAllAsync(int id)
        {
            var reviewer = await _unitOfWork.Reviewers.GetFullReviewerAsync(id);
            if (reviewer == null)
            {
                throw new InvalidOperationException($"Reviewer not found in database with id: {id}");
            }

            return reviewer;
        }

        public async Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync()
        {
            var result = new Result<IEnumerable<Reviewer>>();
            await ResultHandler.TryExecuteAsync(result, GetReviewersAsync(result));
            return result;
        }

        protected async Task<Result<IEnumerable<Reviewer>>> GetReviewersAsync(Result<IEnumerable<Reviewer>> result)
        {
            var reviewers = await _unitOfWork.Reviewers.GetAllAsync();

            ResultHandler.SetOk(reviewers, result);

            return result;
        }

        public async Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer)
        {
            var result = new Result<Reviewer>();
            await ResultHandler.TryExecuteAsync(result, UpdateReviewerAsync(reviewer, result));
            return result;
        }

        public async Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer request, Result<Reviewer> result)
        {
            var getReviewer = await _unitOfWork.Reviewers.GetByIDAsync(request.ReviewerId);

            if (getReviewer == null)
            {
                ResultHandler.SetNotFound(nameof(request.ReviewerId), result);
                return result;
            }

            if (request.NickName != null)
            {
                var nickNameAreNotSame = ResultHandler.CheckStringPropsAreEqual(request.NickName,
                    getReviewer.NickName, nameof(request.NickName), result);

                if (nickNameAreNotSame)
                {
                    var getAnother = await _unitOfWork.Reviewers.GetByNickNameAsync(request.NickName);

                    if (getAnother != null)
                    {
                        ResultHandler.SetExists(nameof(request.NickName), result);
                    }
                    else
                    {
                        getReviewer.NickName = request.NickName;
                    }
                }
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }
            
            ResultHandler.SetOk(getReviewer, result);

            _unitOfWork.Reviewers.Update(getReviewer);
            await _unitOfWork.SaveAsync();

            return result;
        }

        public async Task<Result<IEnumerable<Review>>> GetAllReviewsAsync()
        {
            var result = new Result<IEnumerable<Review>>();
            await ResultHandler.TryExecuteAsync(result, GetAllReviewsAsync(result));
            return result;
        }

        protected async Task<Result<IEnumerable<Review>>> GetAllReviewsAsync(Result<IEnumerable<Review>> result)
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();

            ResultHandler.SetOk(reviews, result);

            return result;
        }

        public async Task<Result<Review>> GetReviewAsync(int id)
        {
            var result = new Result<Review>();

            await ResultHandler.TryExecuteAsync(result, GetReviewAsync(id, result));
            return result;
        }

        public async Task<Result<Review>> GetReviewAsync(int id, Result<Review> result)
        {
            var review = await _unitOfWork.Reviews.GetByIDAsync(id);

            if (review == null)
            {
                ResultHandler.SetNotFound(nameof(review.ReviewId), result);
                return result;
            }

            ResultHandler.SetOk(review, result);

            return result;
        }

        public async Task<Result<Review>> AddReviewAsync(int movieId, int reviewerId, Review review)
        {
            var result = new Result<Review>();

            await ResultHandler.TryExecuteAsync(result, AddReviewAsync(movieId, reviewerId, review, result));
            return result;
        }

        protected async Task<Result<Review>> AddReviewAsync(int movieId, int reviewerId, Review review, Result<Review> result)
        {
            var getReviewer = await _unitOfWork.Reviewers.GetReviewerWithReviewsAsync(reviewerId);
            if (getReviewer == null)
            {
                ResultHandler.SetAccountNotFound(nameof(getReviewer.ReviewerId), result);
                return result;
            }

            var getMovie = await _unitOfWork.Movies.GetByIDAsync(movieId);
            if (getMovie == null)
            {
                ResultHandler.SetNotFound(nameof(getMovie.MovieId), result);
                return result;
            }

            var getReview = getReviewer.Reviews.FirstOrDefault(x => x.MovieId == movieId);
            if (getReview != null)
            {
                ResultHandler.SetExists(nameof(getReview.ReviewerId), result);
                return result;
            }

            getMovie.Reviews.Add(review);

            RecalculateTotalMovieScore(getMovie);

            _unitOfWork.Movies.Update(getMovie);
            await _unitOfWork.SaveAsync();

            return result;
        }


        public async Task<Result<Review>> UpdateReviewAsync(int reviewId, int reviewerId, Review review)
        {
            var result = new Result<Review>();
            await ResultHandler.TryExecuteAsync(result, UpdateReviewAsync(reviewId, reviewerId, review, result));
            return result;
        }

        protected async Task<Result<Review>> UpdateReviewAsync(int reviewId, int reviewerId, Review review, Result<Review> result)
        {
            var getReviewer = await _unitOfWork.Reviewers.GetReviewerWithReviewsAsync(reviewerId);
            if (getReviewer == null)
            {
                ResultHandler.SetAccountNotFound(nameof(getReviewer.ReviewerId), result);
                return result;
            }

            var getReview = await _unitOfWork.Reviews.GetByIDAsync(reviewId);
            if (getReview == null)
            {
                ResultHandler.SetNotFound(nameof(getReview.ReviewerId), result);
                return result;
            }

            var getMovie = await _unitOfWork.Movies.GetByIDAsync(getReview.MovieId);
            if (getMovie == null)
            {
                ResultHandler.SetNotFound(nameof(getMovie.MovieId), result);
                return result;
            }

            if (getReview.ReviewerId != reviewerId)
            {
                result.ResultType = ResultType.Forbidden;
                result.Title = "Cannot update review from another reviewer";
                result.AddError(nameof(getReview.ReviewerId), "Cannot update review from another reviewer");
                return result;
            }

            if (!review.ReviewText.IsNullOrEmpty())
            {
                var reviewsTextAreNotSame = ResultHandler.CheckStringPropsAreEqual(review.ReviewText, getReview.ReviewText,
                    nameof(review.ReviewText), result);

                if (!reviewsTextAreNotSame)
                {
                    getReview.ReviewText = review.ReviewText;
                }
            }

            var canChangeRate = review.Rate != 0 && getReview.Rate != review.Rate;
            if (canChangeRate)
            {
                if (getReview.Rate == review.Rate)
                {
                    result.ResultType = ResultType.AlreadyExists;
                    result.Title = "Please check your input data";
                    result.AddError(nameof(review.Rate), "Old and new Rates are the same!");
                }
                else
                {
                    getReview.Rate = review.Rate;
                    //TODO: recalculate movie rate
                }
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            if (canChangeRate)
            {
                RecalculateTotalMovieScore(getMovie);
            }

            _unitOfWork.Reviews.Update(getReview);
            _unitOfWork.Movies.Update(getMovie);
            await _unitOfWork.SaveAsync();

            ResultHandler.SetOk(getReview, result);

            return result;
        }

        public async Task<Result> DeleteReviewAsync(int reviewerId, int reviewId)
        {
            var result = new Result();
            await ResultHandler.TryExecuteAsync(result, DeleteReviewAsync(reviewerId, reviewId, result));
            return result;
        }

        public async Task<Result> DeleteReviewAsync(int reviewerId, int reviewId, Result result)
        {
            var getReviewer = await _unitOfWork.Reviewers.GetByIDAsync(reviewId);
            if (getReviewer == null)
            {
                ResultHandler.SetAccountNotFound(nameof(getReviewer.ReviewerId), result);
                return result;
            }

            var getReview = await _unitOfWork.Reviews.GetByIDAsync(reviewId);
            if (getReview == null)
            {
                ResultHandler.SetNotFound(nameof(getReview.ReviewId), typeof(Review), result);
                return result;
            }

            if (getReview.ReviewerId != reviewerId)
            {
                result.ResultType = ResultType.Forbidden;
                result.Title = "Cannot delete review from another reviewer";
                result.AddError(nameof(getReview.ReviewerId), "Cannot delete review from another reviewer");
                return result;
            }

            var getMovie = await _unitOfWork.Movies.GetByIDAsync(getReview.MovieId);
            if (getMovie == null)
            {
                ResultHandler.SetNotFound(nameof(getMovie.MovieId), typeof(Movie), result);
                return result;
            }


            
            getMovie.Reviews.Remove(getReview);
            RecalculateTotalMovieScore(getMovie);

            await _unitOfWork.Reviews.DeleteAsync(reviewId);
            _unitOfWork.Movies.Update(getMovie);
            await _unitOfWork.SaveAsync();

            ResultHandler.SetOk(result);

            return result;
        }
    }
}
