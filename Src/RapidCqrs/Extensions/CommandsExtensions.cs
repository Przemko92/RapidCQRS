using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Extensions
{
    internal static class CommandsExtensions
    {
        public static async Task<TResponse> ExecuteAsync<TRequest, TResponse >(this ICommandHandler<TRequest, TResponse> eventHandler, TRequest command)
            where TRequest : ICommand<TResponse>
        {
            return await Task.Run(() => eventHandler.Execute(command));
        }
    }
}
