using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Data.Services.Decorators
{
    public class ProducerServiceDecorator : IProducerService
    {
        private readonly IDbContextFactory<MoviesDBContext> dbContextFactory;

        public ProducerServiceDecorator(IDbContextFactory<MoviesDBContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<Result<Producer>> AddProducerAsync(Producer producer)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new ProducerService(unitOfWork);
                return await service.AddProducerAsync(producer);
            }
        }

        public async Task<Result> DeleteProducerAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new ProducerService(unitOfWork);
                return await service.DeleteProducerAsync(id);
            }
        }

        public async Task<Result<IEnumerable<Producer>>> GetAllProducersAsync()
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new ProducerService(unitOfWork);
                return await service.GetAllProducersAsync();
            }
        }

        public async Task<Result<Producer>> GetProducerAsync(int id)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new ProducerService(unitOfWork);
                return await service.GetProducerAsync(id);
            }
        }

        public async Task<Result<Producer>> UpdateProducerAsync(Producer Producer)
        {
            using (var unitOfWork = new UnitOfWork(dbContextFactory.CreateDbContext()))
            {
                var service = new ProducerService(unitOfWork);
                return await service.UpdateProducerAsync(Producer);
            }
        }
    }
}
