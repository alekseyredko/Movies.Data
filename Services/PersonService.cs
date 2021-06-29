using Movies.Data.Models;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddActorAsync(Actor actor)
        {
            return _unitOfWork.Actors.InsertAsync(actor);
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
