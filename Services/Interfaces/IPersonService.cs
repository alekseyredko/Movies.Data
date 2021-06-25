using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    interface IPersonService
    {
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonAsync(int id);
        Task<IEnumerable<Person>> GetAllPersonAsync();
        Task DeletePersonAsync(int id);
    }
}
