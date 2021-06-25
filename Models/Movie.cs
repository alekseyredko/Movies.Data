using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieGenres = new HashSet<Genre>();
            MoviesActors = new HashSet<Actor>();
            Reviews = new HashSet<Review>();
        }

        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public TimeSpan Duration { get; set; }
        public double? Rate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int ProducerId { get; set; }

        public virtual Producer Producer { get; set; }
        public virtual ICollection<Genre> MovieGenres { get; set; }
        public virtual ICollection<Actor> MoviesActors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
