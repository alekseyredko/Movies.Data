using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Genres = new HashSet<Genre>();
            Actors = new HashSet<Actor>();
            Reviews = new HashSet<Review>();
        }

        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public TimeSpan Duration { get; set; }
        public double? Rate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int ProducerId { get; set; }

        public virtual Producer Producer { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Reviewer> Reviewers { get; set; }
        public List<ReviewerWatchHistory> ReviewerWatchHistories { get; set; }
    }
}
