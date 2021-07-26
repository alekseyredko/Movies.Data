using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using AutoMapper.Mappers;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.Data.DataAccess;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using Movies.Data.Models.Validators;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.Services
{
    public class UserService: IUserService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _userValidator;

        private readonly IDbContextFactory<MoviesDBContext> dbContextFactory;

        public UserService(IDbContextFactory<MoviesDBContext> dbContextFactory, IValidator<User> userValidator)
        {
            this.dbContextFactory = dbContextFactory;
            _userValidator = userValidator;
        }

        public UserService(IUnitOfWork unitOfWork, IValidator<User> userValidator)
        {
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
        }

        private (string, string) HashPasswordAndGetSalt(string password)
        {
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.HashPassword(password, salt);
            return (salt, hash);
        }

        public async Task<Result<User>> GetUserAccountAsync(int id)
        {
            var result = new Result<User>();
            using (_unitOfWork = new UnitOfWork(dbContextFactory))
            {
                await ResultHandler.TryExecuteAsync(result, GetUserAsync(id, result));
            }
            return result;
        }

        protected async Task<Result<User>> GetUserAsync(int id, Result<User> result)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(id);
            if (user == null)
            {
                ResultHandler.SetNotFound(nameof(user.UserId), result);
                return result;
            }

            user.Roles = await GetUserRolesAsync(id);

            ResultHandler.SetOk(user, result);

            return result;
        }

        public async Task<Result<User>> RegisterAsync(User userRequest)
        {
            var result = new Result<User>();
            await ResultHandler.TryExecuteAsync(result, RegisterAsync(userRequest, result));
            return result;
        }

        protected async Task<Result<User>> RegisterAsync(User userRequest, Result<User> result)
        {
            var validationResult = await _userValidator.ValidateAsync(userRequest, options =>
            {
                options.IncludeAllRuleSets();
            });
            if (!validationResult.IsValid)
            {
                ResultHandler.AddInvalidProps(result, validationResult);
                return result;
            }

            var getUser = await _unitOfWork.UserRepository.GetByLoginAsync(userRequest.Login);

            if (getUser != null)
            {
                ResultHandler.SetExists(nameof(getUser.Login), result);
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

        public async Task<Result<User>> LoginAsync(User request)
        {
            var result = new Result<User>();
            await ResultHandler.TryExecuteAsync(result, LoginAsync(request, result));
            return result;
        }

        protected async Task<Result<User>> LoginAsync(User request, Result<User> result)
        {
            var validationResult = await _userValidator.ValidateAsync(request, options =>
            {
                options.IncludeRulesNotInRuleSet();
            });
            if (!validationResult.IsValid)
            {
                ResultHandler.AddInvalidProps(result, validationResult);
                return result;
            }

            var user = await _unitOfWork.UserRepository.GetByLoginAsync(request.Login);
            if (user == null)
            {
                ResultHandler.SetNotFound(nameof(request.Login), result);
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

            return result;
        }


        public async Task<Result<User>> UpdateAccountAsync(User request)
        {
            var result = new Result<User>();
            await ResultHandler.TryExecuteAsync(result, UpdateAccountAsync(request, result));
            return result;
        }

        protected async Task<Result<User>> UpdateAccountAsync(User request, Result<User> result)
        {
            var validationResult = await _userValidator.ValidateAsync(request, options =>
            {
                options.IncludeAllRuleSets();
            });
            if (!validationResult.IsValid)
            {
                ResultHandler.AddInvalidProps(result, validationResult);
                return result;
            }

            var user = await _unitOfWork.UserRepository.GetByIDAsync(request.UserId);
            if (user == null)
            {
                ResultHandler.SetAccountNotFound(nameof(request.Login), result);
                return result;
            }

            var person = await _unitOfWork.Persons.GetByIDAsync(user.UserId);

            if (person == null)
            {
                result.ResultType = ResultType.Unexpected;
                result.Title = "Please check your data";
                return result;
            }

            if (request.Login != null)
            {
                var loginsAreNotSame =
                    ResultHandler.CheckStringPropsAreEqual(request.Login, user.Login, nameof(request.Login),
                        result);

                if (loginsAreNotSame)
                {
                    var getAnother = await _unitOfWork.UserRepository.GetByLoginAsync(request.Login);
                    if (getAnother != null)
                    {
                        ResultHandler.SetExists(nameof(request.Login), result);
                    }
                    else
                    {
                        user.Login = request.Login;
                    }
                }
            }

            if (request.Name != null)
            {
                var namesAreNotSame =
                    ResultHandler.CheckStringPropsAreEqual(request.Name, person.PersonName, nameof(request.Name), 
                        result);

                if (namesAreNotSame)
                {
                    person.PersonName = request.Name;
                }
            }

            if (request.Password != null)
            {
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
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            _unitOfWork.Persons.Update(person);
            _unitOfWork.UserRepository.Update(user);

            await _unitOfWork.SaveAsync();

           
            ResultHandler.SetOk(request, result);

            return result;
        }

        public async Task<Result> DeleteAccountAsync(int id)
        {
            var result = new Result();
            await ResultHandler.TryExecuteAsync(result, DeleteAccountAsync(id, result));
            return result;
        }

        protected async Task<Result> DeleteAccountAsync(int id, Result result)
        {
            var user = await _unitOfWork.UserRepository.GetByIDAsync(id);
            var person = await _unitOfWork.Persons.GetByIDAsync(id);

            if (user == null || person == null)
            {
                ResultHandler.SetAccountNotFound("Id", result);

                return result;
            }

            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            result.Title = "Successfully deleted";

            return result;
        }

        public async Task<IEnumerable<UserRoles>> GetUserRolesAsync(int id)
        {
            var roles = new List<UserRoles>();
            var person = await _unitOfWork.Persons.GetFullPersonAsync(id);

            if (person == null)
            {
                return null;
            }

            roles.Add(UserRoles.Person);

            if (person.Actor != null)
            {
                roles.Add(UserRoles.Actor);
            }

            if (person.Producer !=null)
            {
                roles.Add(UserRoles.Producer);
            }

            if (person.Reviewer != null)
            {
                roles.Add(UserRoles.Reviewer);
            }

            return roles;
        }
    }
}
