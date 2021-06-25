using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }

        public virtual Actor Actor { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
