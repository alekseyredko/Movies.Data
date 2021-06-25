using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Actor
    {
        public Actor()
        {
            Movies = new HashSet<Movie>();
        }

        public int ActorId { get; set; }
        public int? Age { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
