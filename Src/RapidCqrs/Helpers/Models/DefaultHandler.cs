using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Models
{
    public class DefaultHandler : IDefaultHandler
    {
        public object Execute(ICommand<object> request)
        {
            throw new InvalidDataException("This command cannot be published");
        }

        public void Publish(object @event)
        {
            throw new InvalidDataException("This event cannot be published");
        }

        public void Dispose()
        {
        }
    }
}
