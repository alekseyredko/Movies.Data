using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Dto
{
    public class ActorDto
    {
        public ActorDto()
        {
            MoviesActors = new HashSet<MovieDto>();
        }

        public int ActorId { get; set; }
        public int? Age { get; set; }

        public virtual PersonDto ActorNavigation { get; set; }
        public virtual ICollection<MovieDto> MoviesActors { get; set; }
    }
}
