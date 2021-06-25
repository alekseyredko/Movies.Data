using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    interface IActorsService
    {
        Task AddActorAsync(Actor actor);
        Task<Actor> GetActorAsync(int id);
        Task DeleteActorAsync(int id);
    }
}
