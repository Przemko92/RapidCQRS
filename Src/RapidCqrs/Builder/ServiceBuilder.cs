using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Helpers.Models;

namespace RapidCqrs.Builder
{
    public class ServiceBuilder : IServiceBuilder
    {
        private static readonly Type CommandHandlerType = typeof(ICommandHandler<,>);
        private static readonly Type EventHandlerType = typeof(IEventHandler<>);

        private Func<Type, IHandler> _defaultHandler;
        private IDictionary<Type, Func<IHandler>> _handlers;
        private IHandlerResolver _resolver;
        private IContainerRegistration _containerRegistration;
        private List<Type> _handlerTypes;

        public ServiceBuilder()
        {
            this._handlerTypes = new List<Type>();
            this._handlers = new Dictionary<Type, Func<IHandler>>();
        }

        public IServiceBuilder Register(params Type[] handlers)
        {
            var invalidHandlers = handlers.Where(x => x.IsAssignableFrom(typeof(IHandler)));

            if (invalidHandlers.Any())
            {
                throw new InvalidCastException($"Cannot cast types {string.Join(",", invalidHandlers.Select(x => x.Name))} to IHandler");
            }

            foreach (var handler in handlers)
            {
                var commandType = FindCommandType(handler);

                if (this._handlers.ContainsKey(commandType))
                {
                    this._handlers.Remove(commandType);
                    this._handlerTypes.Remove(handler);
                }

                Func<IHandler> resolverFunc = () => (IHandler)this._resolver.Resolve(handler);
                this._handlers.Add(commandType, resolverFunc);
                this._handlerTypes.Add(handler);
                this._containerRegistration?.RegisterAction(handler);
            }

            return this;
        }

        private Type FindCommandType(Type handler)
        {
            var @interface = handler
                .GetInterfaces()
                .First(x => x.Name.Equals(CommandHandlerType.Name) && x.Namespace.Equals(CommandHandlerType.Namespace) ||
                            x.Name.Equals(EventHandlerType.Name) && x.Namespace.Equals(EventHandlerType.Namespace));

            return @interface.GetGenericArguments().First();
        }

        public IServiceBuilder RegisterResolver(IHandlerResolver resolver)
        {
            this._resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            return this;
        }

        public IServiceBuilder AutoRegisterHandlers(params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            var handlers = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IHandler).IsAssignableFrom(x))
                .ToArray();
            Register(handlers);
            return this;
        }

        public IServiceBuilder RegisterDefaultHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            this._defaultHandler = (x) => (IHandler)Activator.CreateInstance(handlerType);
            return this;
        }

        public IServiceBuilder RegisterFactory(Type messageType, Func<IHandler> handlerFactory)
        {
            if (messageType == null) throw new ArgumentNullException(nameof(messageType));
            if (handlerFactory == null) throw new ArgumentNullException(nameof(handlerFactory));

            this._handlers.Add(messageType, handlerFactory);
            return this;
        }

        public IMediator Build()
        {
            if (this._resolver == null)
            {
                this._resolver = new BasicHandlerResolver(handler => (IHandler)Activator.CreateInstance(handler));
            }
            if (this._defaultHandler == null)
            {
                this._defaultHandler = (x) => (IHandler)Activator.CreateInstance(typeof(DefaultHandler<,>).MakeGenericType(x, typeof(object)));
            }

            var handlersFactory = new HandlersFactory(this._handlers, this._defaultHandler);
            var mediator = new Mediator(handlersFactory);
            return mediator;
        }

        public IServiceBuilder RegisterContainer(IContainerRegistration containerRegistration)
        {
            this._containerRegistration = containerRegistration ?? throw new ArgumentNullException(nameof(containerRegistration));

            foreach (var handler in this._handlerTypes)
            {
                this._containerRegistration.RegisterAction(handler);
            }

            return this;
        }
    }
}
