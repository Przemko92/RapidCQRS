using System;
using System.Reflection;
using System.Threading;
using Autofac;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Autofac
{
    public static class AutofacExtenssions
    {
        private static CqrsBuilder _builder;

        public static ICqrsBuilder AddRapidCqrs(this ContainerBuilder containerBuilder)
        {
            _builder = new CqrsBuilder();
            _builder
                .RegisterContainer(new ContainerRegistration(x =>
                    containerBuilder
                        .RegisterType(x)
                        .AsSelf()
                        .InstancePerDependency()));

            containerBuilder
                .Register(x =>
                {
                    var scope = x.Resolve<ILifetimeScope>();
                    _builder.RegisterResolver(new BasicHandlerResolver(scope.Resolve));
                    return _builder.Build();
                })
                .As<IMediator>()
                .SingleInstance();

            return _builder;
        }
    }
}
