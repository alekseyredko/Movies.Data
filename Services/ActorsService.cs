using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services
{
    public class ActorsService : IActorsService
    {       
        private readonly IUnitOfWork _unitOfWork;
       
        public ActorsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task AddActorAsync(Actor actor)
        {           
            await _unitOfWork.Actors.InsertAsync(actor);
            await _unitOfWork.SaveAsync();
        }       

        public async Task DeleteActorAsync(int id)
        {
            await _unitOfWork.Actors.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public Task<Actor> GetActorAsync(int id)
        {
            return _unitOfWork.Actors.GetByIDAsync(id);
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync()
        {
            var actors = await _unitOfWork.Actors.GetAllAsync();
            return actors;
        }

        public async Task<Actor> GetActorWithMoviesAsync(int id)
        {
            var actor = await _unitOfWork.Actors.GetActorWithMoviesAsync(id);

            return actor;
        }

        public async Task<IEnumerable<Actor>> GetAllActorsWithMoviesAsync()
        {
            var actors = await _unitOfWork.Actors.GetAllActorsWithMoviesAsync();

            return actors;
        }

        public async Task DeleteActorFromMovieAsync(int movieId, int actorId)
        {
            var movie = await _unitOfWork.Movies.GetByIDAsync(movieId);
            var actor = await _unitOfWork.Actors.GetActorWithMoviesAsync(actorId);

            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found with id: {movieId}");
            }

            actor.Movies.Remove(movie);
            _unitOfWork.Actors.Update(actor);

            await _unitOfWork.SaveAsync();
        }

        public async Task AddMovieToActorAsync(int movieId, int actorId)
        {
            var movie = await _unitOfWork.Movies.GetByIDAsync(movieId);
            var actor = await _unitOfWork.Actors.GetActorWithMoviesAsync(actorId);

            if (movie == null)
            {
                throw new InvalidOperationException($"Movie not found with id: {movieId}");
            }

            actor.Movies.Add(movie);
            _unitOfWork.Actors.Update(actor);

            await _unitOfWork.SaveAsync();
        }
    }
}
