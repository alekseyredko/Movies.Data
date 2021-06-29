using Movies.Data.Models;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IReviewerRepository: IGenericRepository<Reviewer>
    {
        Task<IEnumerable<Reviewer>> GetAllReviewersWithAllAsync();
        Task<Reviewer> GetReviewerWithReviewsAsync(int reviewerId);
        Task<Reviewer> GetReviewerWithMoviesAsync(int reviewerId);
        Task<Reviewer> GetReviewerWithAllAsync(int reviewerId);
    }
}
