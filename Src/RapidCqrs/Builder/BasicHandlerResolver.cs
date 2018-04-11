using System;
using System.Collections.Generic;
using System.Text;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers.Interfaces;

namespace RapidCqrs.Builder
{
    public class BasicHandlerResolver : IHandlerResolver
    {
        private readonly Func<Type, object> _resolverFunc;

        public BasicHandlerResolver(Func<Type, object> resolverFunc)
        {
            this._resolverFunc = resolverFunc;
        }

        public T Resolve<T>()
        {
            return (T)this._resolverFunc(typeof(T));
        }

        public object Resolve(Type type)
        {
            return this._resolverFunc(type);
        }
    }
}
