using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.temp
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public string Role { get; set; }

        public virtual User User { get; set; }
    }
}
