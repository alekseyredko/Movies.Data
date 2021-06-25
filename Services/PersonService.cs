using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;
using MoviesDataLayer.Models;
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

        public async Task AddPersonAsync(Person person)
        {
            await _unitOfWork.Persons.InsertAsync(person);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            await _unitOfWork.Persons.DeleteAsync(id);
        }

        public Task<Person> GetPersonAsync(int id)
        {
            return _unitOfWork.Persons.GetByIDAsync(id);
        }
    }
}
