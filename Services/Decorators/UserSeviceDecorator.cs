using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Decorators
{
    public class UserSeviceDecorator: IUserService
    {
        private readonly IDbContextFactory<MoviesDBContext> dbContextFactory;
        private readonly IValidator<User> userValidator;

        public UserSeviceDecorator(IDbContextFactory<MoviesDBContext> dbContextFactory, IValidator<User> userValidator)
        {
            this.dbContextFactory = dbContextFactory;
            this.userValidator = userValidator;
        }

        public async Task<Result> DeleteAccountAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new UserService(unitOfWork, userValidator);
                return await service.DeleteAccountAsync(id);
            }            
        }

        public async Task<Result<User>> GetUserAccountAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new UserService(unitOfWork, userValidator);
                return await service.GetUserAccountAsync(id);
            }
        }

        public async Task<IEnumerable<UserRoles>> GetUserRolesAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new UserService(unitOfWork, userValidator);
                return await service.GetUserRolesAsync(id);
            }
        }

        public async Task<Result<User>> LoginAsync(User request)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new UserService(unitOfWork, userValidator);
                return await service.LoginAsync(request);
            }
        }

        public async Task<Result<User>> RegisterAsync(User userRequest)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new UserService(unitOfWork, userValidator);
                return await service.RegisterAsync(userRequest);
            }
        }

        public async Task<Result<User>> UpdateAccountAsync(User request)
        {
            throw new NotImplementedException();
        }
    }   
}
