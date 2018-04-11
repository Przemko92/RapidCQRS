using System.Reflection;
using Autofac;
using RapidCqrs.Autofac;
using RapidCqrs.Builder;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrsApp.Handlers.Commands;
using RapidCqrsApp.Handlers.Events;
using RapidCqrsApp.Models;
using RapidCqrsApp.Models.Commands;
using RapidCqrsApp.Models.Events;

namespace RapidCqrsApp
{
    class Program
    {
        //#################################################################
        /// <summary>
        /// With autofac and autofac integration
        /// </summary>
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder
                .AddRabbitCqrs()
                .Register(typeof(MySimpleCommandHandler)) // Register manualy
                .Register(typeof(MySimpleEventHandler));
            //.AutoRegisterHandlers(Assembly.GetEntryAssembly()); // Register automatically

            containerBuilder
                .RegisterType<TestParam>()
                .AsSelf();

            IContainer container = containerBuilder
                .Build()
                .InitCqrs();

            var mediator = container.Resolve<IMediator>();

            mediator.SendAsync(new MySimpleEvent());
            MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
        }
        //#################################################################



        ////#################################################################
        ///// <summary>
        ///// With any container without integration
        ///// </summary>
        //private static IContainer container;
        //static void Main(string[] args)
        //{
        //    var containerBuilder = new ContainerBuilder();
        //    var cqrsBuilder = new ServiceBuilder();

        //    var mediator = cqrsBuilder
        //        //.Register(typeof(MySimpleCommandHandler)) // Register manualy
        //        //.Register(typeof(MySimpleEventHandler));
        //        .AutoRegisterHandlers(Assembly.GetEntryAssembly()) // Register automatically                
        //        .RegisterResolver(new BasicHandlerResolver(x => container.Resolve(x)))
        //        .RegisterContainer(new ContainerRegistration(x => containerBuilder.RegisterType(x).AsSelf()))
        //        .Build();

        //    containerBuilder
        //        .Register(x => mediator)
        //        .As<IMediator>()
        //        .SingleInstance();

        //    containerBuilder
        //        .RegisterType<TestParam>()
        //        .AsSelf();

        //    container = containerBuilder
        //        .Build();

        //    mediator.SendAsync(new MySimpleEvent());
        //    MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
        //}
        ////#################################################################



        ////#################################################################
        ///// <summary>
        ///// Without container
        ///// </summary>
        //static void Main(string[] args)
        //{
        //    var cqrsBuilder = new ServiceBuilder();

        //    var mediator = cqrsBuilder
        //        //.Register(typeof(MySimpleCommandHandler)) // Register manualy
        //        //.Register(typeof(MySimpleEventHandler));
        //        .AutoRegisterHandlers(Assembly.GetEntryAssembly()) // Register automatically                
        //        .Build();

        //    // mediator.SendAsync(new MySimpleEvent()); // No parametreless constructor exception
        //    MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
        //}
        ////#################################################################
    }
}
