using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IEventHandler<in TRequest> : IHandler<TRequest>
    {
        void Publish(TRequest @event);
    }

    public interface IAsyncEventHandler<in TRequest> : IHandler<TRequest>
    {
        Task Publish(TRequest @event, CancellationToken cancellationToken = default);
    }
}
