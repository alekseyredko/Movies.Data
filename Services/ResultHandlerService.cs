using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;

namespace Movies.Data.Services
{
    public class ResultHandlerService: IResultHandlerService
    {
        public async Task<Result<T>> HandleTaskAsync<T>(Result<T> result, Task<Result<T>> func)
        {
            try
            {
                await func;
            }
            catch (Exception e)
            {
                //TODO: log exception
                result.ResultType = ResultType.Unexpected;
                result.Value = default(T);
                result.Title = "Sorry, please try again later!";
            }
            return result;
        }

        public async Task<Result> HandleTaskAsync(Result result, Task<Result> func)
        {
            try
            {
                await func;
            }
            catch (Exception e)
            {
                //TODO: log exception
                result.ResultType = ResultType.Unexpected;
                result.Title = "Sorry, please try again later!";
            }
            return result;
        }

        public bool CheckStringPropsAreEqual<T>(string prop1, string prop2, string propName, Result<T> result)
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

        public void SetNotFound<T>(string propName, Result<T> result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such {typeof(T).Name} found! Check your {propName}");
        }

        public void SetNotFound(string propName, Type type, Result result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such {type.Name} found! Check your {propName}");
        }

        public void SetAccountNotFound(string propName, Result result)
        {
            result.ResultType = ResultType.NotFound;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"No such account found! Check your {propName}");
        }

        public void SetExists<T>(string propName, Result<T> result)
        {
            result.ResultType = ResultType.AlreadyExists;
            result.Title = "Please check your entered data";
            result.AddError(propName, $"Another {typeof(T).Name} with such {propName} already exists!");
        }

        public void SetOk<T>(T value, Result<T> result)
        {
            result.Value = value;
            result.ResultType = ResultType.Ok;
            result.Title = "Success!";
        }

        public void SetOk(Result result)
        {
            result.ResultType = ResultType.Ok;
            result.Title = "Success!";
        }
    }
}
