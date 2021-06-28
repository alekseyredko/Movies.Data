using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Repositiories
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        public ActorRepository(MoviesDBContext context) : base(context)
        {
              
        }

        public async Task<Actor> GetActorWithMoviesAsync(int id)
        {
            var actor = await context.Actors.SingleAsync(x => x.ActorId == id);
            await context.Entry(actor).Collection(x => x.Movies).LoadAsync();

            return actor;
        }

        public async Task<IEnumerable<Actor>> GetAllActorsWithMoviesAsync()
        {
            var actors = await context.Actors.Include(x => x.Movies).ToListAsync();
            return actors;
        }
    }
}
