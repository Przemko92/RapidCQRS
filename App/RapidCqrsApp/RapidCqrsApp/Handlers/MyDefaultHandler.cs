using System;
using System.Collections.Generic;
using System.Text;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrsApp.Handlers
{
    class MyDefaultHandler : IDefaultHandler
    {
        public object Execute(ICommand<object> request)
        {
            return null;
        }

        public void Publish(IEvent @event)
        {
        }

        public void Dispose()
        {
        }
    }
}
