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
    }
}
