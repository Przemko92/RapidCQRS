using RapidCqrs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface ICommandHandler
    {
        
    }

    public interface IAsyncCommandHandler
    {
        
    }

    public interface ICommandHandler<in TRequest, out TResponse> : IHandler<TRequest>, ICommandHandler
        where TRequest : ICommand<TResponse>
    {
        TResponse Execute(TRequest request);
    }

    public interface IAsyncCommandHandler<in TRequest, TResponse> : IHandler<TRequest>, IAsyncCommandHandler
        where TRequest : ICommand<TResponse>
    {
        Task<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default);
    }
}
