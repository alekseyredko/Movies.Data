using Movies.Data.Models;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IPersonRepository: IGenericRepository<Person>
    {
        Task<IEnumerable<Person>> GetAllFullPersonAsync();
        Task<IEnumerable<Person>> GetAllPersonWithActorsAsync();
        Task<Person> GetPersonWithActorAsync(int id);
        //Task<IEnumerable<Person>> GetPersonWithProducerAsync();
        //Task<IEnumerable<Person>> GetPesonWithReviewersAsync();
        //Task<Actor> GetActorAsync(int id);
        //Task<Producer> GetProducerAsync(int id);
        //Task<Reviewer> GetReviewerAsync(int id);
        Task AddPersonAsync(Person person);
        //Task AddActorAsync(Actor actor);
        //Task AddProducerAsync(Producer producer);
        //Task AddReviewerAsync(Reviewer reviewer);

    }
}
