using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;
using Movies.Data.Services.Interfaces;

namespace Movies.Data.Services
{
    public class JWTTokenService: ITokenService
    {

        public string GenerateToken(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
