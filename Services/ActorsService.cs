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

        public Task AddActorAsync(Actor actor)
        {
            return _unitOfWork.Actors.InsertAsync(actor);
        }

        public Task DeleteActorAsync(int id)
        {
            return _unitOfWork.Actors.DeleteAsync(id);
        }

        public Task<Actor> GetActorAsync(int id)
        {
            return _unitOfWork.Actors.GetByIDAsync(id);
        }
    }
}
