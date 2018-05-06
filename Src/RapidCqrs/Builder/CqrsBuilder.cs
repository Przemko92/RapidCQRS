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
    public class CqrsBuilder : ICqrsBuilder
    {
        private static readonly Type CommandHandlerType = typeof(ICommandHandler<,>);
        private static readonly Type CommandAsyncHandlerType = typeof(IAsyncCommandHandler<,>);
        private static readonly Type EventHandlerType = typeof(IEventHandler<>);
        private static readonly Type EventAsyncHandlerType = typeof(IAsyncEventHandler<>);

        private Func<IHandler> _defaultHandler;
        private IDictionary<Type, Func<IHandler>> _handlers;
        private IHandlerResolver _resolver;
        private IContainerRegistration _containerRegistration;
        private List<Type> _handlerTypes;

        public CqrsBuilder()
        {
            this._handlerTypes = new List<Type>();
            this._handlers = new Dictionary<Type, Func<IHandler>>();
            this.RegisterDefaultHandler(typeof(DefaultHandler));
        }

        public ICqrsBuilder Register<T>()
        {
            return Register(typeof(T));
        }

        public ICqrsBuilder Register(params Type[] handlers)
        {
            var invalidHandlers = handlers.Where(x => !typeof(IHandler).IsAssignableFrom(x));

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
                            x.Name.Equals(EventHandlerType.Name) && x.Namespace.Equals(EventHandlerType.Namespace) ||
                            x.Name.Equals(CommandAsyncHandlerType.Name) && x.Namespace.Equals(CommandAsyncHandlerType.Namespace) ||
                            x.Name.Equals(EventAsyncHandlerType.Name) && x.Namespace.Equals(EventAsyncHandlerType.Namespace));

            return @interface.GetGenericArguments().First();
        }

        public ICqrsBuilder RegisterResolver(IHandlerResolver resolver)
        {
            this._resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            return this;
        }

        public ICqrsBuilder RegisterDefaultHandler<T>()
        {
            return RegisterDefaultHandler(typeof(T));
        }

        public ICqrsBuilder AutoRegisterHandlers(params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            var handlers = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IHandler).IsAssignableFrom(x))
                .ToArray();
            Register(handlers);
            return this;
        }

        public ICqrsBuilder RegisterDefaultHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            if (!typeof(IDefaultHandler).IsAssignableFrom(handlerType))
            {
                throw new InvalidCastException($"Cannot cast type {handlerType} to IDefaultHandler");
            }

            this._defaultHandler = () => (IHandler)this._resolver.Resolve(handlerType);
            this._containerRegistration?.RegisterAction(handlerType);
            this._handlerTypes.Add(handlerType);
            return this;
        }
        
        public ICqrsBuilder RegisterContainer(IContainerRegistration containerRegistration)
        {
            this._containerRegistration = containerRegistration ?? throw new ArgumentNullException(nameof(containerRegistration));

            foreach (var handler in this._handlerTypes)
            {
                this._containerRegistration.RegisterAction(handler);
            }

            return this;
        }

        public IMediator Build()
        {
            if (this._resolver == null)
            {
                this._resolver = new BasicHandlerResolver(handler => (IHandler)Activator.CreateInstance(handler));
            }

            var handlersFactory = new HandlersFactory(this._handlers, this._defaultHandler);
            var mediator = new Mediator(handlersFactory);
            return mediator;
        }
    }
}
