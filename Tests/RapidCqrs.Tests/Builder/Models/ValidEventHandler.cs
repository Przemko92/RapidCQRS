using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Tests.Builder.Models.Commands;

namespace RapidCqrs.Tests.Builder.Models
{
    class ValidEventHandler : IEventHandler<TestEvent>
    {
        public void Publish(TestEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
