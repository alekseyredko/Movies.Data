using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services
{
    class PersonService : IPersonService
    {
        private IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddActorAsync(Actor actor)
        {
            throw new NotImplementedException();
        }

        public async Task AddPersonAsync(Person person)
        {
            await _unitOfWork.Persons.InsertAsync(person);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            await _unitOfWork.Persons.DeleteAsync(id);
        }

        public async Task<IEnumerable<Person>> GetAllPersonAsync()
        {
            var people = await _unitOfWork.Persons.GetAllAsync();
            return people;
        }

        public Task<Person> GetPersonAsync(int id)
        {
            return _unitOfWork.Persons.GetByIDAsync(id);
        }

        public Task<IEnumerable<Person>> GetAllPersonWithActorsAsync()
        {
            return _unitOfWork.Persons.GetAllPersonWithActorsAsync();
        }

        public Task<Person> GetPersonWithActorAsync(int id)
        {
            return _unitOfWork.Persons.GetPersonWithActorAsync(id);
        }
    }
}
