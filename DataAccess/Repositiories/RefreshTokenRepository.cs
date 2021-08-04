using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.DataAccess.Repositiories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MoviesDBContext context) : base(context)
        {            
        }

        public async Task<IEnumerable<RefreshToken>> GetRefreshTokensByUserId(int userId)
        {
            var tokens = await context.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
            return tokens;
        }

        public async Task SetAllUserTokensRevoked(int userId)
        {
            await context.RefreshTokens.Where(x => x.UserId == userId)
                .ForEachAsync(x => x.IsRevoked = true);
        }
    }
}
