using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Builder.Interfaces
{
    public interface IServiceBuilder
    {
        IServiceBuilder Register(params Type[] handlers);
        IServiceBuilder AutoRegisterHandlers(params Assembly[] assemblies);
        IServiceBuilder RegisterDefaultHandler(Type handlerType);
        IServiceBuilder RegisterFactory(Type messageType, Func<IHandler> handlerFactory);
        IServiceBuilder RegisterContainer(IContainerRegistration containerRegistration);
        IServiceBuilder RegisterResolver(IHandlerResolver resolver);
        IMediator Build();
    }
}
