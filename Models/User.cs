using System;
using System.Collections.Generic;

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

        public virtual Person Person { get; set; }
    }
}
