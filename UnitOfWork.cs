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

        private IGenericRepository<Actor> _actorRepository;
        private IGenericRepository<Genre> _genreRepository;
        private IGenericRepository<Movie> _movieRepository;
        private IGenericRepository<Person> _personRepository;
        private IGenericRepository<Review> _reviewRepository;
        private IGenericRepository<Reviewer> _reviewerRepository;
        private IGenericRepository<ReviewerWatchHistory> _reviewerWatchHistoryRepository;

        public UnitOfWork(MoviesDBContext moviesDBContext)
        {
            _moviesDBContext = moviesDBContext;
        }

        public IGenericRepository<Actor> Actors => 
            _actorRepository ??= new GenericRepository<Actor>(_moviesDBContext);

        public IGenericRepository<Genre> Genres => 
            _genreRepository ??= new GenericRepository<Genre>(_moviesDBContext);

        public IGenericRepository<Movie> Movies => 
            _movieRepository ??= new GenericRepository<Movie>(_moviesDBContext);

        public IGenericRepository<Person> Persons => 
            _personRepository ??= new GenericRepository<Person>(_moviesDBContext);

        public IGenericRepository<Review> Reviews => 
            _reviewRepository ??= new GenericRepository<Review>(_moviesDBContext);

        public IGenericRepository<Reviewer> Reviewers => 
            _reviewerRepository ??= new GenericRepository<Reviewer>(_moviesDBContext);

        public IGenericRepository<ReviewerWatchHistory> ReviewersWatchHistory => 
            _reviewerWatchHistoryRepository ??= new GenericRepository<ReviewerWatchHistory>(_moviesDBContext);

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
