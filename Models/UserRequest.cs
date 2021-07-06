using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Models
{
    public class UserRequest
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
