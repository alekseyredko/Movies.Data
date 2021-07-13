using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;

namespace Movies.Data.DataAccess.Repositiories
{
    public class ProducerRepository: GenericRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(MoviesDBContext context) : base(context)
        {
        }
    }
}
