using AutoMapper;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer;
using MoviesDataLayer.Interfaces;
using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services
{
    public class ActorsService : IActorsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActorsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
