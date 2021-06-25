using MoviesDataLayer.Interfaces;
using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private MoviesDBContext _moviesDBContext;

        private readonly IGenericRepository<Actor> _actorRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IGenericRepository<Person> _personRepository;
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IGenericRepository<Reviewer> _reviewerRepository;
        private readonly IGenericRepository<ReviewerWatchHistory> _reviewerWatchHistoryRepository;

        public IGenericRepository<Actor> Actors => _actorRepository;

        public IGenericRepository<Genre> Genres => _genreRepository;

        public IGenericRepository<Movie> Movies => _movieRepository;

        public IGenericRepository<Person> Persons => _personRepository;

        public IGenericRepository<Review> Reviews => _reviewRepository;

        public IGenericRepository<Reviewer> Reviewers => _reviewerRepository;

        public IGenericRepository<ReviewerWatchHistory> ReviewersWatchHistory => _reviewerWatchHistoryRepository;

        //public UnitOfWork(MoviesDBContext moviesDBContext)
        //{
        //    _moviesDBContext = moviesDBContext;
        //}

        public UnitOfWork(IGenericRepository<Actor> actorRepository, 
            IGenericRepository<Genre> genreRepository, 
            IGenericRepository<Movie> movieRepository, 
            IGenericRepository<Person> personRepository, 
            IGenericRepository<Review> reviewRepository, 
            IGenericRepository<Reviewer> reviewerRepository, 
            IGenericRepository<ReviewerWatchHistory> reviewerWatchHistoryRepository)
        {
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _reviewerWatchHistoryRepository = reviewerWatchHistoryRepository;
        }

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
