using System;
using System.Collections.Generic;
using System.Text;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IHandler<in TRequest> : IHandler
    {
    }

    public interface IHandler : IDisposable
    {
    }
}
