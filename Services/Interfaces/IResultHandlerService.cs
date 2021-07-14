using System;
using System.Threading.Tasks;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IResultHandlerService
    {
        bool CheckStringPropsAreEqual<T>(string prop1, string prop2, string propName, Result<T> result);
        void SetNotFound<T>(string propName, Result<T> result);
        void SetOk<T>(T value, Result<T> result);
        void SetExists<T>(string propName, Result<T> result);
        void SetAccountNotFound(string propName, Result result);
        void SetOk(Result result);
        void SetNotFound(string propName, Type type, Result result);
        Task<Result<T>> HandleTaskAsync<T>(Result<T> result, Task<Result<T>> func);
        Task<Result> HandleTaskAsync(Result result, Task<Result> func);
    }
}