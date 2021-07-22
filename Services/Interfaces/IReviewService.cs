using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Result<Reviewer>> AddReviewerAsync(Reviewer reviewer);
        Task<Result<Reviewer>> GetReviewerAsync(int id);
       Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync();
        Task<Result> DeleteReviewerAsync(int id);
        Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer);

        Task<Result<Review>> AddReviewAsync(int movieId, int reviewerId , Review review);
        Task<Result<IEnumerable<Review>>> GetAllReviewsAsync();
        Task<Result<Review>> GetReviewAsync(int id);
        Task<Result> DeleteReviewAsync(int reviewerId, int reviewId);
        Task<Result<Review>> UpdateReviewAsync(int reviewId, int reviewerId, Review review);
        Task<Result<IEnumerable<Review>>> GetMovieReviewsAsync(int movieId);
        Task<Result<IEnumerable<Review>>> GetReviewerReviewsAsync(int reviewerId);
    }
}
