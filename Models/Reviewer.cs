using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Reviewer
    {
        public Reviewer()
        {
            Reviews = new HashSet<Review>();
        }

        public int ReviewerId { get; set; }
        public double? PersonRate { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public ICollection<Movie> Movies { get; set; }
        public List<ReviewerWatchHistory> ReviewerWatchHistories { get; set; }
    }
}
