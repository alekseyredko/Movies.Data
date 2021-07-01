using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;

namespace Movies.Data.DataAccess.Repositiories
{
    class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MoviesDBContext context) : base(context)
        {
        }
    }
}