using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class Actor
    {
        public Actor()
        {
            MoviesActors = new HashSet<MoviesActor>();
        }

        public int ActorId { get; set; }
        public int? Age { get; set; }

        public virtual Person ActorNavigation { get; set; }
        public virtual ICollection<MoviesActor> MoviesActors { get; set; }
    }
}
