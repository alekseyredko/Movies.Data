using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;

namespace Movies.Data.DataAccess.Repositiories
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        public UserRepository(MoviesDBContext context) : base(context)
        {
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            return user;
        }
    }
}
