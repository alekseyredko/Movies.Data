﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> RegisterAsync(User userRequest);
        Task<Result<User>> LoginAsync(User request);
    }
}
