using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
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
        private readonly IResultHandlerService _resultHandlerService;

        public UserService(IUnitOfWork unitOfWork, IResultHandlerService resultHandlerService)
        {
            _unitOfWork = unitOfWork;
            _resultHandlerService = resultHandlerService;
        }

        private (string, string) HashPasswordAndGetSalt(string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            return (salt, hash);
        }

        public Task<Result<User>> RegisterAsync(User userRequest)
        {
            return _resultHandlerService.HandleTaskAsync(userRequest, RegisterAsync);
        }

        protected async Task<Result<User>> RegisterAsync(User userRequest, Result<User> result)
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
            
            return result;
        }

        public Task<Result<User>> LoginAsync(User request)
        {
            return _resultHandlerService.HandleTaskAsync(request, LoginAsync);
        }

        protected async Task<Result<User>> LoginAsync(User request, Result<User> result)
        {
            var user = await _unitOfWork.UserRepository.GetByLoginAsync(request.Login);
            if (user == null)
            {
                return ReturnUserNotFoundResult(result);
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

            return result;
        }

        private static Result<User> ReturnUserNotFoundResult(Result<User> result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(nameof(User.Login), "No such account found! Check your login.");
            return result;
        }

        public Task<Result<User>> UpdateAccountAsync(User request)
        {
            return _resultHandlerService.HandleTaskAsync(request, UpdateAccountAsync);
        }

        protected async Task<Result<User>> UpdateAccountAsync(User request, Result<User> result)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(request.UserId);
            if (user == null)
            {
                return ReturnUserNotFoundResult(result);
            }

            var person = await _unitOfWork.Persons.GetByIDAsync(user.UserId);

            if (person == null)
            {
                result.ResultType = ResultType.Unexpected;
                result.Title = "Please check your data";
                return result;
            }

            if (!string.IsNullOrEmpty(user.Login) && user.Login == request.Login)
            {
                result.ResultType = ResultType.AlreadyExists;
                result.Title = "Please check your data";
                result.AddError("Login", "Old and new login are the same");
            }
            else if (!string.IsNullOrEmpty(request.Login))
            {
                user.Login = request.Login;
            }

            if (!string.IsNullOrEmpty(user.Password) &&
                user.PasswordHash == SecurityHelper.HashPassword(request.Password, user.PasswordSalt))
            {
                result.ResultType = ResultType.AlreadyExists;
                result.Title = "Please check your data";
                result.AddError("Password", "Old and new password are the same");
            }
            else if (!string.IsNullOrEmpty(request.Password))
            {
                var salt = SecurityHelper.GenerateSalt();
                var hash = SecurityHelper.HashPassword(request.Password, salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
            }

            if (!string.IsNullOrEmpty(user.Name) &&
                    request.Name == person.PersonName)
            {
                result.ResultType = ResultType.AlreadyExists;
                result.Title = "Please check your data";
                result.AddError("Name", "Old and new name are the same");
            }
            else if (!string.IsNullOrEmpty(request.Name))
            {
                person.PersonName = request.Name;
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            _unitOfWork.Persons.Update(person);
            _unitOfWork.UserRepository.Update(user);

            await _unitOfWork.SaveAsync();

            result.Value = request;
            result.Title = "Successful update!";

            return result;
        }

        public Task<Result<int>> DeleteAccountAsync(int id)
        {
            return _resultHandlerService.HandleTaskAsync(id, DeleteAccountAsync);
        }

        protected async Task<Result<int>> DeleteAccountAsync(int id, Result<int> result)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(id);
            var person = await _unitOfWork.Persons.GetByIDAsync(id);

            result.Value = id;
            if (user == null || person == null)
            {
                result.Title = "Please check your data";
                result.AddError(nameof(id), "No such account!");

                return result;
            }

            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            result.Title = "Successfully deleted";

            return result;
        }
    }
}
