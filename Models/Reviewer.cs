using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class Reviewer
    {
        public Reviewer()
        {
            Reviews = new HashSet<Review>();
            ReviewerWatchHistories = new HashSet<ReviewerWatchHistory>();
        }

        public int ReviewerId { get; set; }
        public double? PersonRate { get; set; }

        public virtual Person ReviewerNavigation { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ReviewerWatchHistory> ReviewerWatchHistories { get; set; }
    }
}
