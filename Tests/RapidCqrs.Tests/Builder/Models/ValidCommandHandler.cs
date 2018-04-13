using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Tests.Builder.Models.Commands;

namespace RapidCqrs.Tests.Builder.Models
{
    class ValidCommandHandler : ICommandHandler<TestCommand, object>
    {
        public object Execute(TestCommand request)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
