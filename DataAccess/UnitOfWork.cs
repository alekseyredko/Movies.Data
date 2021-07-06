﻿using Movies.Data.DataAccess;
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
        private readonly IGenreRepository _genreRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IReviewerWatchHistoryRepository _reviewerWatchHistoryRepository;
        private readonly IUserRepository _userRepository;

        public IActorRepository Actors => _actorRepository;

        public IGenreRepository Genres => _genreRepository;

        public IMovieRepository Movies => _movieRepository;

        public IPersonRepository Persons => _personRepository;

        public IReviewRepository Reviews => _reviewRepository;

        public IReviewerRepository Reviewers => _reviewerRepository;

        public IReviewerWatchHistoryRepository ReviewersWatchHistory => _reviewerWatchHistoryRepository;
        public IUserRepository UserRepository => _userRepository;

        //public UnitOfWork(MoviesDBContext moviesDBContext)
        //{
        //    _moviesDBContext = moviesDBContext;
        //}

        public UnitOfWork(IActorRepository actorRepository,
            IGenreRepository genreRepository,
            IMovieRepository movieRepository,
            IPersonRepository personRepository,
            IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IReviewerWatchHistoryRepository reviewerWatchHistoryRepository, 
            MoviesDBContext moviesDBContext, IUserRepository userRepository)
        {
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _movieRepository = movieRepository;
            _personRepository = personRepository;
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _reviewerWatchHistoryRepository = reviewerWatchHistoryRepository;
            _moviesDBContext = moviesDBContext;
            _userRepository = userRepository;
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
