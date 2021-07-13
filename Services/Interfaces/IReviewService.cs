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
        Task<Result> DeleteReviewerAsync(int id);
        Task<Result<Reviewer>> GetReviewerAsync(int id);
        Task<Reviewer> GetReviewerWithAllAsync(int id);
        Task AddReviewAsync(Review review, int movieId);
        Task DeleteReviewAsync(int id);
        Task<Review> GetReviewAsync(int id);
        Task<Result<IEnumerable<Reviewer>>> GetAllReviewersAsync();
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Result<Reviewer>> UpdateReviewerAsync(Reviewer reviewer);
        Task UpdateReviewAsync(Review review);
    }
}
