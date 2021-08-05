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
        Task<RefreshToken> GetRefreshTokenByTokenValue(string token);
        Task<IEnumerable<RefreshToken>> GetRefreshTokensByUserId(int userId);
        Task<User> GetUserByRefreshToken(int tokenId);
        Task SetAllUserTokensRevoked(int userId);
    }
}
