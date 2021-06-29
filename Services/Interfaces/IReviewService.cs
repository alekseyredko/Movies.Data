using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewerAsync(Reviewer reviewer);
        Task DeleteReviewerAsync(int id);
        Task<Reviewer> GetReviewerAsync(int id);
        Task AddReviewAsync(Review review, int movieId);
        Task DeleteReviewAsync(int id);
        Task<Review> GetReviewAsync(int id);
    }
}
