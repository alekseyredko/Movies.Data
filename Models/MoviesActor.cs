using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class MoviesActor
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public virtual Actor Actor { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
