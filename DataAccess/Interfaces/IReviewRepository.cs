using Movies.Data.Models;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IReviewRepository: IGenericRepository<Review>
    {        
        Task<Review> GetReviewWithMovie(int reviewId);
        Task<Review> GetReviewWithReviewer(int reviewId);
        Task<Review> GetReviewWithAllAsync(int reviewId);
        Task<IEnumerable<Review>> GetReviewsWithAllAsync();
        Task<IEnumerable<Review>> GetMovieReviewsAsync(int movieId);
        Task<IEnumerable<Review>> GetReviewerReviewsAsync(int reviewerId);
    }
}
