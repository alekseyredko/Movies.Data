using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movies.Data.Models;
using Movies.Data.Results;

namespace Movies.Data.Services.Interfaces
{
    public interface IProducerService
    {
        Task<Result<IEnumerable<Producer>>> GetAllProducersAsync();
        Task<Result<Producer>> GetProducerAsync(int id);
        Task<Result<Producer>> AddProducerAsync(Producer producer);
        Task<Result<Producer>> UpdateProducerAsync(Producer Producer);
        Task<Result> DeleteProducerAsync(int id);
    }
}
