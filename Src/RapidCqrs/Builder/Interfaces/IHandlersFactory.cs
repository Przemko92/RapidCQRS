using System;
using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Builder.Interfaces
{
    public interface IHandlersFactory
    {
        IHandler GetHandler(Type requestType);
        Task<IHandler> GetHandlerAsync(Type requestType, CancellationToken cancellationToken = default);
    }
}