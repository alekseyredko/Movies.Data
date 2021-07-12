﻿using System;
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

        public async Task<Result<T>> HandleTaskAsync<T>(T request, Func<T, Result<T>, Task<Result<T>>> func)
        {
            var result = new Result<T>();
            try
            {
                result = await func(request, result);
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

        public async Task<Result<T>> HandleTaskAsync<T>(Func<Result<T>, Task<Result<T>>> func)
        {
            var result = new Result<T>();
            try
            {
                result = await func(result);
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
    }
}
