using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using Microsoft.IdentityModel.Tokens;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> RegisterAsync(UserRequest userRequest)
        {
            //TODO: find by login and throw exception

            var person = new Person
            {
                PersonName = userRequest.Name
            };
            await _unitOfWork.Persons.InsertAsync(person);
            //await _unitOfWork.Persons.AddPersonAsync(person);
            await _unitOfWork.SaveAsync();

            (string, string) hash = HashPasswordAndGetSalt(userRequest.Password);

            var user = new User
            {
                Person = person,
                NickName = userRequest.Login,
                PasswordSalt = hash.Item1,
                PasswordHash = hash.Item2
            };

            await _unitOfWork.UserRepository.InsertAsync(user);

            var userResponse = new UserResponse
            {
                Id = user.UserId,
                //Token = GetToken()
            };

            await _unitOfWork.SaveAsync();
            return userResponse;
        }
        
        private (string, string) HashPasswordAndGetSalt(string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            return (salt, hash);
        }
        public async Task<UserResponse> LoginAsync(UserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByLoginAsync(request.Login);
            if (user == null)
            {
                throw new InvalidOperationException($"No such users with login: {request.Login}");
            }

            bool isVerified = SecurityHelper.IsVerified(request.Password, user.PasswordSalt, user.PasswordHash);
            if (!isVerified)
            {
                throw new InvalidOperationException("Invalid password!");
            }

            var userResponse = new UserResponse
            {
                Id = user.UserId,
                //Token = GetToken()
            };

            return userResponse;
        }
    }
}
