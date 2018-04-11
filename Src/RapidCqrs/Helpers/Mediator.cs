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
            using (var handler = await this._handlersFactory.GetHandlerAsync(request.GetType(), cancellationToken))
            {
                return (TResponse)handler.GetType().GetMethod("Execute").Invoke(handler, new[] { request });
            }
        }

        public TResponse Execute<TResponse>(ICommand<TResponse> request)
        {
            using (var handler = this._handlersFactory.GetHandler(request.GetType()))
            {
                return (TResponse)handler.GetType().GetMethod("Execute").Invoke(handler, new[] { request });
            }
        }

        public async Task SendAsync<TRequest>(TRequest @event, CancellationToken cancellationToken = default)
            where TRequest : IEvent
        {
            using (var handler = (IEventHandler<TRequest>)await this._handlersFactory.GetHandlerAsync(@event.GetType(), cancellationToken))
            {
                await handler.PublishAsync(@event);
            }
        }

        public void Send<TRequest>(TRequest @event)
            where TRequest : IEvent
        {
            using (var handler = (IEventHandler<TRequest>)this._handlersFactory.GetHandler(@event.GetType()))
            {
                handler.Publish(@event);
            }
        }
    }
}
