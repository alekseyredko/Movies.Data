using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieGenres = new HashSet<MovieGenre>();
            MoviesActors = new HashSet<MoviesActor>();
            ProducerMovies = new HashSet<ProducerMovie>();
            Reviews = new HashSet<Review>();
        }

        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public TimeSpan Duration { get; set; }
        public double? Rate { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<MoviesActor> MoviesActors { get; set; }
        public virtual ICollection<ProducerMovie> ProducerMovies { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
