using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetByLoginAsync(string login);
    }
}
