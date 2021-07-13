using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IResultHandlerService
    {
        Task<Result<T>> HandleTaskAsync<T>(T request, Func<T, Result<T>, Task<Result<T>>> func);
        Task<Result<T>> HandleTaskAsync<T, T1>(T1 request, Func<T1, Result<T>, Task<Result<T>>> func);
        Task<Result<T>> HandleTaskAsync<T>(Func<Result<T>, Task<Result<T>>> func);
        bool CheckStringPropsAreEqual<T>(string prop1, string prop2, string propName, Result<T> result);
        void SetNotFound<T>(string propName, Result<T> result);
        void SetOk<T>(T value, Result<T> result);
        void SetExists<T>(string propName, Result<T> result);
        Task<Result> HandleTaskAsync<T>(T request, Func<T, Result, Task<Result>> func);
        void SetAccountNotFound(string propName, Result result);
        void SetOk(Result result);
    }
}
