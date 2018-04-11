using System;
using System.Reflection;
using Autofac;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Autofac
{
    public static class AutofacExtenssions
    {
        private static ServiceBuilder _builder;
        private static IMediator _mediator;

        public static IServiceBuilder AddRabbitCqrs(this ContainerBuilder containerBuilder)
        {
            _builder = new ServiceBuilder();
            _builder
                .RegisterContainer(new ContainerRegistration(x => containerBuilder.RegisterType(x).AsSelf()));

            containerBuilder
                .Register(x => _mediator)
                .As<IMediator>()
                .SingleInstance();

            return _builder;
        }
        
        public static IContainer InitCqrs(this IContainer container)
        {
            _builder
                .RegisterResolver(new BasicHandlerResolver(container.Resolve));

            _mediator = _builder.Build();
            return container;
        }
    }
}
