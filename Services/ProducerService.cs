using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Movies.Data.DataAccess;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.Services
{
    public class ProducerService: IProducerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDbContextFactory<MoviesDBContext> dbContextFactory;

        public ProducerService(IDbContextFactory<MoviesDBContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public ProducerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<Producer>>> GetAllProducersAsync()
        {
            var result = new Result<IEnumerable<Producer>>();
            await ResultHandler.TryExecuteAsync(result, GetAllProducersAsync(result));
            return result;
        }

        protected async Task<Result<IEnumerable<Producer>>> GetAllProducersAsync(Result<IEnumerable<Producer>> result)
        {
            var Producers = await _unitOfWork.Producers.GetAllAsync();

            ResultHandler.SetOk(Producers, result);

            return result;
        }

        public async Task<Result<Producer>> GetProducerAsync(int id)
        {
            var result = new Result<Producer>();
            await ResultHandler.TryExecuteAsync(result, GetProducerAsync(id, result));
            return result;
        }

        protected async Task<Result<Producer>> GetProducerAsync(int id, Result<Producer> result)
        {
            var Producer = await _unitOfWork.Producers.GetByIDAsync(id);
            if (Producer == null)
            {
                ResultHandler.SetNotFound(nameof(Producer.ProducerId), result);
                return result;
            }

            ResultHandler.SetOk(Producer, result);

            return result;
        }

        public async Task<Result<Producer>> AddProducerAsync(Producer producer)
        {
            var result = new Result<Producer>();
            await ResultHandler.TryExecuteAsync(result, AddProducerAsync(producer, result));
            return result;
        }

        protected async Task<Result<Producer>> AddProducerAsync(Producer request, Result<Producer> result)
        {
            var Producer = await _unitOfWork.Producers.GetByIDAsync(request.ProducerId);

            if (Producer != null)
            {
                ResultHandler.SetExists(nameof(request.ProducerId), result);
            }

            if (request.Country.IsNullOrEmpty())
            {
                result.ResultType = ResultType.NotValid;
                result.AddError(nameof(request.Country), $"Field {nameof(request.Country)} mustn't be empty!");
                result.Title = "Check your entered data!";
            }

            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            await _unitOfWork.Producers.InsertAsync(request);
            await _unitOfWork.SaveAsync();

            result.ResultType = ResultType.Ok;
            result.Value = request;
            result.Title = "Success!";

            return result;
        }


        public async Task<Result<Producer>> UpdateProducerAsync(Producer Producer)
        {
            var result = new Result<Producer>();
            await ResultHandler.TryExecuteAsync(result, UpdateProducerAsync(Producer, result));
            return result;
        }

        public async Task<Result<Producer>> UpdateProducerAsync(Producer request, Result<Producer> result)
        {
            var getProducer = await _unitOfWork.Producers.GetByIDAsync(request.ProducerId);

            if (getProducer == null)
            {
                ResultHandler.SetNotFound(nameof(request.ProducerId), result);
                return result;
            }

            if (request.Country != null)
            {
                var countryAreNotSame = ResultHandler.CheckStringPropsAreEqual(request.Country,
                    getProducer.Country, nameof(request.Country), result);

                if (countryAreNotSame)
                {
                    getProducer.Country = request.Country;
                }
            }


            if (result.ResultType != ResultType.Ok)
            {
                return result;
            }

            ResultHandler.SetOk(getProducer, result);

            _unitOfWork.Producers.Update(getProducer);
            await _unitOfWork.SaveAsync();

            return result;
        }

        public async Task<Result> DeleteProducerAsync(int id)
        {
            var result = new Result();
            await ResultHandler.TryExecuteAsync(result, DeleteProducerAsync(id, result));
            return result;
        }

        public async Task<Result> DeleteProducerAsync(int id, Result result)
        {
            var producer = await _unitOfWork.Producers.GetByIDAsync(id);
            if (producer == null)
            {
                ResultHandler.SetAccountNotFound("Id", result);
                return result;
            }

            await _unitOfWork.Producers.DeleteAsync(id);

            await _unitOfWork.SaveAsync();

            ResultHandler.SetOk(result);

            return result;
        }
    }
}
