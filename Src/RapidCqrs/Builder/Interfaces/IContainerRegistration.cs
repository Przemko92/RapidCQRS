using System;
using System.Collections.Generic;
using System.Text;

namespace RapidCqrs.Builder.Interfaces
{
    public interface IContainerRegistration
    {
        Action<Type> RegisterAction { get; }
    }
}
