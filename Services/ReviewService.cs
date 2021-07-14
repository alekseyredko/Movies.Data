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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Results;

namespace Movies.Data.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResultHandlerService _resultHandlerService;
        public ReviewService(IUnitOfWork unitOfWork, IResultHandlerService resultHandlerService)
        {
            _unitOfWork = unitOfWork;
            _resultHandlerService = resultHandlerService;
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

        public Task<Result<Reviewer>> AddReviewerAsync(Reviewer reviewer)
        {
            var result = new Result<Reviewer>();
            return _resultHandlerService.HandleTaskAsync(result, AddReviewerAsync(reviewer, result));
        }

        protected async Task<Result<Reviewer>> AddReviewerAsync(Reviewer request, Result<Reviewer> result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(request.ReviewerId);

            if (reviewer != null)
            {
                _resultHandlerService.SetExists(nameof(request.ReviewerId), result);
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

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetReviewWithMovie(id);
            if (review == null)
            {
                throw new InvalidOperationException($"No such review with id: {id}");
            }

            var movie = await _unitOfWork.Movies.GetMovieWithReviewsAsync(review.MovieId);

            await _unitOfWork.Reviews.DeleteAsync(id);
            movie.Reviews.Remove(review);

            RecalculateTotalMovieScore(movie);

            _unitOfWork.Movies.Update(movie);           
            await _unitOfWork.SaveAsync();
        }

        public Task<Result> DeleteReviewerAsync(int id)
        {
            var result = new Result();
            return _resultHandlerService.HandleTaskAsync(result, DeleteReviewerAsync(id, result));
        }

        public async Task<Result> DeleteReviewerAsync(int id, Result result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(id);
            if (reviewer == null)
            {
                _resultHandlerService.SetAccountNotFound("Id", result);
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

            _resultHandlerService.SetOk(result);

            return result;
        }

        protected void RecalculateTotalMovieScore(Movie movie)
        {            
            movie.Rate = movie.Reviews.Sum(x => x.Rate);
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

        public Task<Result<Reviewer>> GetReviewerAsync(int id)
        {
            var result = new Result<Reviewer>();
            return _resultHandlerService.HandleTaskAsync(result, GetReviewerAsyncResult(id, result));
        }

        protected async Task<Result<Reviewer>> GetReviewerAsyncResult(int id, Result<Reviewer> result)
        {
            var reviewer = await _unitOfWork.Reviewers.GetByIDAsync(id);
            if (reviewer == null)
            {
                _resultHandlerService.SetNotFound(nameof(reviewer.ReviewerId), result);
                return result;
            }

            _resultHandlerService.SetOk(reviewer, result);

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

        public Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync()
        {
            var result = new Result<IEnumerable<Reviewer>>();
            return _resultHandlerService.HandleTaskAsync(result, GetReviewersAsync(result));
        }

        protected async Task<Result<IEnumerable<Reviewer>>> GetReviewersAsync(Result<IEnumerable<Reviewer>> result)
        {
            var reviewers = await _unitOfWork.Reviewers.GetAllAsync();

            _resultHandlerService.SetOk(reviewers, result);

            return result;
        }

        public Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return _unitOfWork.Reviews.GetAllAsync();
        }

        public Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer)
        {
            var result = new Result<Reviewer>();
            return _resultHandlerService.HandleTaskAsync(result, UpdateReviewerAsync(reviewer, result));
        }

        public async Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer request, Result<Reviewer> result)
        {
            var getReviewer = await _unitOfWork.Reviewers.GetByIDAsync(request.ReviewerId);

            if (getReviewer == null)
            {
                _resultHandlerService.SetNotFound(nameof(request.ReviewerId), result);
                return result;
            }

            if (request.NickName != null)
            {
                var nickNameAreNotSame = _resultHandlerService.CheckStringPropsAreEqual(request.NickName,
                    getReviewer.NickName, nameof(request.NickName), result);

                if (nickNameAreNotSame)
                {
                    var getAnother = await _unitOfWork.Reviewers.GetByNickNameAsync(request.NickName);

                    if (getAnother != null)
                    {
                        _resultHandlerService.SetExists(nameof(request.NickName), result);
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
            
            _resultHandlerService.SetOk(getReviewer, result);

            _unitOfWork.Reviewers.Update(getReviewer);
            await _unitOfWork.SaveAsync();

            return result;
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.SaveAsync();
        }
    }
}
