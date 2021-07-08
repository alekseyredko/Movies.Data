using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using Microsoft.IdentityModel.Tokens;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using Movies.Data.Results;
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

        public async Task<Result<User>> RegisterAsync(User userRequest)
        {
            var result = new Result<User>();
            try
            {
                var getUser = await _unitOfWork.UserRepository.GetByLoginAsync(userRequest.Login);

                if (getUser != null)
                {
                    result.Value = userRequest;
                    result.ResultType = ResultType.AlreadyExists;
                    result.Title = "Please check your entered data";
                    result.AddError(nameof(getUser.Login), "Account with this login already exists!");
                    return result;
                }

                var person = new Person
                {
                    PersonName = userRequest.Name
                };

                await _unitOfWork.Persons.InsertAsync(person);

                await _unitOfWork.SaveAsync();

                (string, string) hash = HashPasswordAndGetSalt(userRequest.Password);

                var user = new User
                {
                    Person = person,
                    Login = userRequest.Login,
                    PasswordSalt = hash.Item1,
                    PasswordHash = hash.Item2
                };

                await _unitOfWork.UserRepository.InsertAsync(user);

                var userResponse = new User
                {
                    UserId = user.UserId,
                };

                await _unitOfWork.SaveAsync();

                result.Value = userResponse;
                result.Title = "Registration successful!";
            }
            catch (Exception e)
            {
                //TODO: log exception
                result.ResultType = ResultType.Unexpected;
                result.Value = null;
                result.Title = "Sorry, please try again later!";
            }

            return result;
        }

        private (string, string) HashPasswordAndGetSalt(string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            return (salt, hash);
        }
        public async Task<Result<User>> LoginAsync(User request)
        {
            var result = new Result<User>();
            try
            {
                var user = await _unitOfWork.UserRepository.GetByLoginAsync(request.Login);
                if (user == null)
                {
                    result.ResultType = ResultType.NotFound;
                    result.Title = "Please check your entered data";
                    result.AddError(nameof(user.Login), "No such account found! Check your login.");
                    return result;
                }

                result.Value = user;

                bool isVerified = SecurityHelper.IsVerified(request.Password, user.PasswordSalt, user.PasswordHash);
                if (!isVerified)
                {
                    result.ResultType = ResultType.NotValid;
                    result.Title = "Please check your entered data";
                    result.AddError(nameof(user.Password), "Invalid password!");
                    return result;
                }
                result.Title = "Login successful!";
            }
            catch (Exception e)
            {
                //TODO: log exception
                result.ResultType = ResultType.Unexpected;
                result.Value = null;
                result.Title = "Sorry, please try again later!";
            }
            return result;
        }
    }
}
