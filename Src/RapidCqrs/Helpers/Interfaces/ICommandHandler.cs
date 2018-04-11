using RapidCqrs.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface ICommandHandler<in TRequest, out TResponse> : IHandler<TRequest>
        where TRequest : ICommand<TResponse>
    {
        TResponse Execute(TRequest request);
    }
}
