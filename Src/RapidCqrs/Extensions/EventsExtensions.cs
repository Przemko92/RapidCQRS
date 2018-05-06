using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Extensions
{
    internal static class EventsExtensions
    {
        public static async Task PublishAsync<TRequest>(this IEventHandler<TRequest> eventHandler, TRequest @event, CancellationToken cancellationToken = default ) 
        {
            await Task.Run(() => eventHandler.Publish(@event), cancellationToken);
        }
    }
}
