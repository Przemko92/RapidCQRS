using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Builder
{
    internal class HandlersFactory : IHandlersFactory
    {
        private readonly IDictionary<Type, Func<IHandler>> _handlersDictionary;
        private readonly Func<Type, IHandler> _defaultHandler;

        internal HandlersFactory(IDictionary<Type, Func<IHandler>> handlersDictionary, Func<Type, IHandler> defaultHandler)
        {
            this._handlersDictionary = handlersDictionary;
            this._defaultHandler = defaultHandler;
        }

        public IHandler GetHandler(Type requestType)
        {
            IHandler handler;

            if (this._handlersDictionary.ContainsKey(requestType))
            {
                handler = this._handlersDictionary[requestType]();
            }
            else if (this._defaultHandler != null)
            {
                handler = this._defaultHandler(requestType);
            }
            else
            {
                throw new InvalidDataException($"Cannot find handler for type {requestType.Name}");
            }

            return handler;
        }

        public async Task<IHandler> GetHandlerAsync(Type requestType, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => GetHandler(requestType), cancellationToken);
        }
    }
}
