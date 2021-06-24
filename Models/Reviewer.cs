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
        }

        public int ReviewerId { get; set; }
        public double? PersonRate { get; set; }

        public virtual Person ReviewerNavigation { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
