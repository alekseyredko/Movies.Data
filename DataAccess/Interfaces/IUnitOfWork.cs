using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IActorRepository Actors { get; }
        IGenreRepository Genres { get; }
        IMovieRepository Movies { get; }
        IPersonRepository Persons { get; }
        IReviewRepository Reviews { get; }
        IReviewerRepository Reviewers { get; }
       IReviewerWatchHistoryRepository ReviewersWatchHistory { get; }
        Task<int> SaveAsync();
    }
}
