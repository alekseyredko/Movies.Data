using Movies.Data.DataAccess;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesDataLayer
{

    //TODO: implment CQRS
    //TODO: get rid of Unit of work
    //inject repos directly in Services
    //implement specific repository interfaces
    public class UnitOfWork : IUnitOfWork
    {
        private MoviesDBContext _moviesDBContext;

        private readonly IActorRepository _actorRepository;
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IGenericRepository<ReviewerWatchHistory> _reviewerWatchHistoryRepository;

        public IActorRepository Actors => _actorRepository;

        public IGenericRepository<Genre> Genres => _genreRepository;

        public IMovieRepository Movies => _movieRepository;

        public IPersonRepository Persons => _personRepository;

        public IReviewRepository Reviews => _reviewRepository;

        public IReviewerRepository Reviewers => _reviewerRepository;

        public IGenericRepository<ReviewerWatchHistory> ReviewersWatchHistory => _reviewerWatchHistoryRepository;

        //public UnitOfWork(MoviesDBContext moviesDBContext)
        //{
        //    _moviesDBContext = moviesDBContext;
        //}

        public UnitOfWork(IActorRepository actorRepository,
            IGenericRepository<Genre> genreRepository,
            IMovieRepository movieRepository,
            IPersonRepository personRepository,
            IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IGenericRepository<ReviewerWatchHistory> reviewerWatchHistoryRepository, 
            MoviesDBContext moviesDBContext)
        {
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _reviewerWatchHistoryRepository = reviewerWatchHistoryRepository;
            _moviesDBContext = moviesDBContext;
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
