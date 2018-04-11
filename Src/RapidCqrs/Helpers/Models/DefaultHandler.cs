using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Models
{
    internal class DefaultHandler<TRequest, TResponse>  : ICommandHandler<TRequest, TResponse>, IEventHandler<TRequest> 
        where TRequest : ICommand<TResponse>
    {
        public TResponse Execute(TRequest request)
        {
            throw new ArgumentException("Cannot parse this request");
        }

        public Task<TResponse> ExecuteAsync(TRequest request)
        {
            throw new ArgumentException("Cannot parse this request");
        }

        public void Publish(TRequest @event)
        {
            throw new ArgumentException("Cannot parse this event");
        }

        public Task PublishAsync(TRequest @event)
        {
            throw new ArgumentException("Cannot parse this event");
        }

        public void Dispose()
        {
        }
    }
}
