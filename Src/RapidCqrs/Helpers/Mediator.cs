using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Extensions;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers
{
    public class Mediator : IMediator
    {
        private readonly IHandlersFactory _handlersFactory;

        public Mediator(IHandlersFactory handlersFactory)
        {
            this._handlersFactory = handlersFactory;
        }

        public async Task<TResponse> ExecuteAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handler = this._handlersFactory.GetHandler(request.GetType());

            if (handler is IAsyncCommandHandler)
            {
                try
                {
                    return await (Task<TResponse>)handler.GetType().GetMethod("Execute").Invoke(handler, new object[] { request, cancellationToken });
                }
                finally
                {
                    handler.Dispose();
                }
            }
            else
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        return (TResponse)handler.GetType().GetMethod("Execute")
                            .Invoke(handler, new object[] { request });
                    }
                    finally
                    {
                        handler.Dispose();
                    }
                }, cancellationToken);
            }
        }

        public TResponse Execute<TResponse>(ICommand<TResponse> request)
        {
            var handler = this._handlersFactory.GetHandler(request.GetType());
            try
            {
                if (handler is ICommandHandler)
                {
                    return (TResponse)handler.GetType().GetMethod("Execute").Invoke(handler, new object[] { request });
                }
                else
                {
                    var result =
                        ((Task<TResponse>)handler.GetType().GetMethod("Execute").Invoke(handler, new object[] { request, default(CancellationToken) }));

                    return result != null ? result.Result : default(TResponse);
                }
            }
            finally
            {
                handler.Dispose();
            }
        }

        public async Task SendAsync<TRequest>(TRequest @event, CancellationToken cancellationToken = default)
        {
            var handler = this._handlersFactory.GetHandler(@event.GetType());
            if (handler is IEventHandler<TRequest>)
            {
                await Task.Run(() =>
                 {
                     try
                     {
                         ((IEventHandler<TRequest>)handler).Publish(@event);
                     }
                     finally
                     {
                         handler.Dispose();
                     }

                 }, cancellationToken);
            }
            else
            {
                try
                {
                    await ((IAsyncEventHandler<TRequest>)handler).Publish(@event, cancellationToken);
                }
                finally
                {
                    handler.Dispose();
                }
            }
        }

        public void Send<TRequest>(TRequest @event)
        {
            var handler = this._handlersFactory.GetHandler(@event.GetType());
            try
            {
                if (handler is IEventHandler<TRequest>)
                {
                    ((IEventHandler<TRequest>)handler).Publish(@event);
                }
                else
                {
                    ((IAsyncEventHandler<TRequest>)handler).Publish(@event).RunSynchronously();
                }
            }
            finally
            {
                handler.Dispose();
            }
        }
    }
}
