using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;

namespace Movies.Data.DataAccess.Repositiories
{
    class ReviewerWatchHistoryRepository : GenericRepository<ReviewerWatchHistory>, IReviewerWatchHistoryRepository
    {
        public ReviewerWatchHistoryRepository(MoviesDBContext context) : base(context)
        {
        }
    }
}