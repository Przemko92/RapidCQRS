using System;
using System.Collections.Generic;
using System.Text;

namespace RapidCqrs.Builder.Interfaces
{
    public interface IHandlerResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}
