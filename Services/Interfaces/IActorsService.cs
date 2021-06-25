using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    interface IActorsService
    {
        Task AddActorAsync(Actor actor);
        Task<Actor> GetActorAsync(int id);
        Task<IEnumerable<Actor>> GetActorsAsync();
        Task DeleteActorAsync(int id);
    }
}
