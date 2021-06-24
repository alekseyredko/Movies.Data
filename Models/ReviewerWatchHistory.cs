using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class ReviewerWatchHistory
    {
        public int MovieId { get; set; }
        public int ReviewerId { get; set; }
        public DateTime? LastWatchTime { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
