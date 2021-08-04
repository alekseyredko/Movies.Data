using Movies.Data.Models;
using MoviesDataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<IEnumerable<RefreshToken>> GetRefreshTokensByUserId(int userId);
        Task SetAllUserTokensRevoked(int userId);
    }
}
