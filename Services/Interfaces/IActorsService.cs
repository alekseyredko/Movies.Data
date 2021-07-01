using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IActorsService
    {
        Task AddActorAsync(Actor actor);
        Task<Actor> GetActorAsync(int id);
        Task<Actor> GetActorWithMoviesAsync(int id);
        Task<IEnumerable<Actor>> GetActorsAsync();
        Task DeleteActorAsync(int id);
        Task<IEnumerable<Actor>> GetAllActorsWithMoviesAsync();
        Task AddMovieToActorAsync(int movieId, int actorId);
        Task DeleteActorFromMovieAsync(int movieId, int actorId);
        
    }
}
