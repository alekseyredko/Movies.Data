using MoviesDataLayer.Interfaces;
using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer
{
    class UnitOfWork : IUnitOfWork
    {
        private MoviesDBContext _moviesDBContext;

        public UnitOfWork(MoviesDBContext moviesDBContext)
        {
            _moviesDBContext = moviesDBContext;
        }

        public IGenericRepository<Actor> Actors => throw new NotImplementedException();

        public IGenericRepository<Genre> Genres => throw new NotImplementedException();

        public IGenericRepository<Movie> Movies => throw new NotImplementedException();

        public IGenericRepository<Person> Persons => throw new NotImplementedException();

        public IGenericRepository<Review> Reviews => throw new NotImplementedException();

        public IGenericRepository<Reviewer> Reviewers => throw new NotImplementedException();

        public IGenericRepository<ReviewerWatchHistory> ReviewersWatchHistory => throw new NotImplementedException();

        public void Dispose()
        {
            _moviesDBContext.Dispose();
        }

        public Task<int> SaveAsync()
        {
            return _moviesDBContext.SaveChangesAsync();
        }
    }
}
