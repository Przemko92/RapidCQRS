using System.Threading.Tasks;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IEventHandler<in TRequest> : IHandler<TRequest>
        where TRequest : IEvent
    {
        void Publish(TRequest @event);
    }
}
