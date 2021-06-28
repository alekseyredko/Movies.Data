using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IPersonService
    {
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonAsync(int id);
        Task<Person> GetPersonWithActorAsync(int id);
        Task<IEnumerable<Person>> GetAllPersonAsync();
        Task DeletePersonAsync(int id);
        Task<IEnumerable<Person>> GetAllPersonWithActorsAsync();       
        Task AddActorAsync(Actor actor);

    }
}
