using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Movies.Data.Models;
using Movies.Data.Results;
using Movies.Data.Services.Interfaces;
using MoviesDataLayer.Interfaces;

namespace Movies.Data.Services
{
    public class ProducerService: IProducerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResultHandlerService _resultHandlerService;

        public ProducerService(IUnitOfWork unitOfWork, IResultHandlerService resultHandlerService)
        {
            _unitOfWork = unitOfWork;
            _resultHandlerService = resultHandlerService;
        }

        public Task<Result<IEnumerable<Producer>>> GetAllProducersAsync()
        {
            return _resultHandlerService.HandleTaskAsync<IEnumerable<Producer>>(GetAllProducersAsync);
        }

        protected async Task<Result<IEnumerable<Producer>>> GetAllProducersAsync(Result<IEnumerable<Producer>> result)
        {
            var Producers = await _unitOfWork.Producers.GetAllAsync();

            _resultHandlerService.SetOk(Producers, result);

            return result;
        }

        public Task<Result<Producer>> GetProducerAsync(int id)
        {
            return _resultHandlerService.HandleTaskAsync<Producer, int>(id, GetProducerAsync);
        }

        protected async Task<Result<Producer>> GetProducerAsync(int id, Result<Producer> result)
        {
            var Producer = await _unitOfWork.Producers.GetByIDAsync(id);
            if (Producer == null)
            {
                _resultHandlerService.SetNotFound(nameof(Producer.ProducerId), result);
                return result;
            }

            _resultHandlerService.SetOk(Producer, result);

            return result;
        }

        public Task<Result<Producer>> AddProducerAsync(Producer producer)
        {
            return _resultHandlerService.HandleTaskAsync(producer, AddProducerAsync);
        }

        protected async Task<Result<Producer>> AddProducerAsync(Producer request, Result<Producer> result)
        {
            var Producer = await _unitOfWork.Producers.GetByIDAsync(request.ProducerId);

            if (Producer != null)
            {
                _resultHandlerService.SetExists(nameof(request.ProducerId), result);
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


        public Task<Result<Producer>> UpdateProducerAsync(Producer Producer)
        {
            return _resultHandlerService.HandleTaskAsync(Producer, UpdateProducerAsync);
        }

        public async Task<Result<Producer>> UpdateProducerAsync(Producer request, Result<Producer> result)
        {
            var getProducer = await _unitOfWork.Producers.GetByIDAsync(request.ProducerId);

            if (getProducer == null)
            {
                _resultHandlerService.SetNotFound(nameof(request.ProducerId), result);
                return result;
            }

            if (request.Country != null)
            {
                var countryAreNotSame = _resultHandlerService.CheckStringPropsAreEqual(request.Country,
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

            _resultHandlerService.SetOk(getProducer, result);

            _unitOfWork.Producers.Update(getProducer);
            await _unitOfWork.SaveAsync();

            return result;
        }

        public Task<Result> DeleteProducerAsync(int id)
        {
            return _resultHandlerService.HandleTaskAsync(id, DeleteProducerAsync);
        }

        public async Task<Result> DeleteProducerAsync(int id, Result result)
        {
            var producer = await _unitOfWork.Producers.GetByIDAsync(id);
            if (producer == null)
            {
                _resultHandlerService.SetAccountNotFound("Id", result);
                return result;
            }

            await _unitOfWork.Producers.DeleteAsync(id);

            await _unitOfWork.SaveAsync();

            _resultHandlerService.SetOk(result);

            return result;
        }
    }
}
