using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Actor
    {
        public Actor()
        {
            MoviesActors = new HashSet<MoviesActor>();
        }

        public int ActorId { get; set; }
        public int? Age { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<MoviesActor> MoviesActors { get; set; }
    }
}
