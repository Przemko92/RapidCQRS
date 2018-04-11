using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Extensions
{
    public static class EventsExtensions
    {
        public static async Task PublishAsync<TRequest>(this IEventHandler<TRequest> eventHandler, TRequest @event)
        {
            await Task.Run(() => eventHandler.Publish(@event));
        }
    }
}
