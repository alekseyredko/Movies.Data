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
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MoviesDBContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Person>> GetAllFullPersonAsync()
        {
            var people = await context.People
                .Include(x => x.Actor)
                .Include(x => x.Producer)
                .Include(x => x.Reviewer)
                .ToListAsync();
            return people;
        }

        public async Task<IEnumerable<Person>> GetAllPersonWithActorsAsync()
        {
            var people = await context.People
                .Include(x => x.Actor)
                .ToListAsync();
            return people;
        }
        
        public async Task AddPersonAsync(Person person)
        {
            await context.People.AddAsync(person);
        }

        public async Task<Person> GetPersonWithActorAsync(int id)
        {
            var person = await context.People.SingleAsync(x => x.PersonId == id);
            await context.Entry(person).Reference(x => x.Actor).LoadAsync();
            
            return person;
        }
    }
}
