using Movies.Data.Models;
using MoviesDataLayer;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IActorRepository: IGenericRepository<Actor>
    {        
        Task<Actor> GetActorWithMoviesAsync(int id);
        Task<IEnumerable<Actor>> GetAllActorsWithMoviesAsync();        
    }
}
