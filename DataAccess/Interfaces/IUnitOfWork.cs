using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<Actor> Actors { get; }
        IGenericRepository<Genre> Genres { get; }
        IGenericRepository<Movie> Movies { get; }
        IGenericRepository<Person> Persons { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Reviewer> Reviewers { get; }
        IGenericRepository<ReviewerWatchHistory> ReviewersWatchHistory { get; }
        Task<int> SaveAsync();
    }
}
