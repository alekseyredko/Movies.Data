using System;
using System.Collections.Generic;

#nullable disable

namespace MoviesDataLayer.Models
{
    public partial class Person
    {
        public Person()
        {
            ProducerMovies = new HashSet<ProducerMovie>();
        }

        public int PersonId { get; set; }
        public string PersonName { get; set; }

        public virtual Actor Actor { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual ICollection<ProducerMovie> ProducerMovies { get; set; }
    }
}
