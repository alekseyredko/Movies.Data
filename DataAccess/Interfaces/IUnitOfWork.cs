using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IActorRepository Actors { get; }
        IGenreRepository Genres { get; }
        IMovieRepository Movies { get; }
        IPersonRepository Persons { get; }
        IProducerRepository Producers { get; }
        IReviewRepository Reviews { get; }
        IReviewerRepository Reviewers { get; }
        IReviewerWatchHistoryRepository ReviewersWatchHistory { get; }
        IUserRepository UserRepository { get; }
        Task<int> SaveAsync();
    }
}
