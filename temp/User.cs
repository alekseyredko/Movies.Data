using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.temp
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
