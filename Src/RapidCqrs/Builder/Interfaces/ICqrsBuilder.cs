using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Builder.Interfaces
{
    public interface ICqrsBuilder
    {
        ICqrsBuilder Register<T>() where T : IHandler;
        ICqrsBuilder Register(params Type[] handlers);
        ICqrsBuilder AutoRegisterHandlers(params Assembly[] assemblies);
        ICqrsBuilder RegisterDefaultHandler(Type handlerType);
        ICqrsBuilder RegisterContainer(IContainerRegistration containerRegistration);
        ICqrsBuilder RegisterResolver(IHandlerResolver resolver);
        ICqrsBuilder RegisterDefaultHandler<T>() where T : IDefaultHandler;
        IMediator Build();
    }
}
