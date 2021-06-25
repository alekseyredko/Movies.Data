using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Dto
{
    public class MovieDto
    {
        public MovieDto()
        {
            MovieGenres = new HashSet<GenreDto>();
            MoviesActors = new HashSet<ActorDto>();
            ProducerMovies = new HashSet<ProducerMovieDto>();
            Reviews = new HashSet<ReviewDto>();
        }

        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public TimeSpan Duration { get; set; }
        public double? Rate { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual ICollection<GenreDto> MovieGenres { get; set; }
        public virtual ICollection<ActorDto> MoviesActors { get; set; }
        public virtual ICollection<ProducerMovieDto> ProducerMovies { get; set; }
        public virtual ICollection<ReviewDto> Reviews { get; set; }
    }
}
