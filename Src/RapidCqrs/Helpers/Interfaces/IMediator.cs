using RapidCqrs.Models.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IMediator
    {
        Task<TResponse> ExecuteAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);            

        TResponse Execute<TResponse>(ICommand<TResponse> request);

        Task SendAsync<TRequest>(TRequest @event, CancellationToken cancellationToken = default) 
            where TRequest : IEvent;

        void Send<TRequest>(TRequest @event) 
            where TRequest : IEvent;
    }
}