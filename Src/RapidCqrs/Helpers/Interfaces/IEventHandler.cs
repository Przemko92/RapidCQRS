using System.Threading.Tasks;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IEventHandler<in TRequest> : IHandler<TRequest>
    {
        void Publish(TRequest @event);
    }
}
