using System;
using System.Collections.Generic;
using System.Data;

#nullable disable

namespace Movies.Data.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<UserRoles> Roles { get; set; }

        public virtual Person Person { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public User()
        {
            Roles = new HashSet<UserRoles> {UserRoles.Person};
        }
    }

    public enum UserRoles
    {
        Person = 1,
        Actor = 2,
        Producer = 4,
        Reviewer = 8,
    }
}
