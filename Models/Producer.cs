using System;
using System.Collections.Generic;

#nullable disable

namespace Movies.Data.Models
{
    public partial class Producer
    {
        public Producer()
        {
            Movies = new HashSet<Movie>();
        }

        public string Country { get; set; }
        public int ProducerId { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
