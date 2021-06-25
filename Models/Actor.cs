using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Actor
    {
        public Actor()
        {
            MoviesActors = new HashSet<Movie>();
        }

        public int ActorId { get; set; }
        public int? Age { get; set; }

        public virtual Person ActorNavigation { get; set; }
        public virtual ICollection<Movie> MoviesActors { get; set; }
    }
}
