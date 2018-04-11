using System;
using System.Collections.Generic;
using System.Text;
using RapidCqrs.Builder.Interfaces;

namespace RapidCqrs.Builder
{
    public class ContainerRegistration : IContainerRegistration
    {
        public Action<Type> RegisterAction { get; }

        public ContainerRegistration(Action<Type> registerAction)
        {
            this.RegisterAction = registerAction;
        }
    }
}
