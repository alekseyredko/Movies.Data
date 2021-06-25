using MoviesDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
