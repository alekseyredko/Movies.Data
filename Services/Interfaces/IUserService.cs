using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;

namespace Movies.Data.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(UserRequest userRequest);
        Task<UserResponse> LoginAsync(UserRequest request);
    }
}
