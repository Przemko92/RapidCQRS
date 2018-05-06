using System;
using System.Collections.Generic;
using System.Text;
using RapidCqrs.Models.Interfaces;

namespace RapidCqrs.Helpers.Interfaces
{
    public interface IDefaultHandler : ICommandHandler<ICommand<object>, object>, IEventHandler<object>
    {
    }
}
