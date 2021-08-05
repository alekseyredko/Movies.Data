using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using MoviesDataLayer;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.Results
{
    public class ResultHandler
    {
        protected static readonly Dictionary<ResultType, string> _errorsDictionary;

        static ResultHandler()
        {
            _errorsDictionary = new Dictionary<ResultType, string>
            {
                {ResultType.Ok, "Success!"},
                {ResultType.AlreadyExists, "Please check your entered data"}
            };
        }

        public static async Task TryExecuteAsync(Result result, Task func)
        {
            try
            {
                await func;
            }

            catch (Exception e)
            {
                //TODO: log exception
                System.Diagnostics.Debug.WriteLine(e.Message);
                if (e.InnerException!=null)
                {
                    System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                }
                result.ResultType = ResultType.Unexpected;
                result.Title = "Sorry, please try again later!";
            }
        }        

        public static bool CheckStringPropsAreEqual<T>(string prop1, string prop2, string propName, Result<T> result)
        {
            if (string.IsNullOrEmpty(prop1))
            {
                result.ResultType = ResultType.NotValid;
                result.AddError(propName, $"{propName} mustn't be empty!");
                return false;
            }
            else if (prop1.Equals(prop2, StringComparison.OrdinalIgnoreCase))
            {
                result.ResultType = ResultType.AlreadyExists;
                result.Title = "Please check your data";
                result.AddError(propName, $"Old and new {propName} are the same");
                return false;
            }

            return true;
        }

        public static void SetForbidden(string propName, Result result)
        {
            result.ResultType = ResultType.Forbidden;
            result.Title = "Forbidden";
            result.AddError(propName, $"Please login");
        }

        public static void SetNotFound<T>(string propName, Result<T> result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such {typeof(T).Name} found! Check your {propName}");
        }

        public static void SetNotFound(string propName, Type type, Result result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such {type.Name} found! Check your {propName}");
        }

        public static void SetAccountNotFound(string propName, Result result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such account found! Check your {propName}");
        }

        public static void SetExists<T>(string propName, Result<T> result)
        {
            result.ResultType = ResultType.AlreadyExists;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"Another {typeof(T).Name} with such {propName} already exists!");
        }

        public static void SetOk<T>(T value, Result<T> result)
        {
            result.Value = value;
            result.ResultType = ResultType.Ok;
            result.Title = "Success!";
        }

        public static void SetOk(Result result)
        {
            result.ResultType = ResultType.Ok;
            result.Title = "Success!";
        }

        public static void AddInvalidProps(Result result, ValidationResult validationResult)
        {
            result.AddErrors(validationResult.Errors);
            result.ResultType = ResultType.NotValid;
            result.Title = "Please check input data";
        }
    }
}
