using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string RevievText { get; set; }
        public double? Rate { get; set; }
        public int MovieId { get; set; }
        public int ReviewerId { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
