using System.Reflection;
using Autofac;
using RapidCqrs.Autofac;
using RapidCqrs.Builder;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrsApp.Handlers;
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
                .AddRapidCqrs()
                .Register(typeof(MySimpleCommandHandler)) // Register manualy
                .Register<MySimpleAsyncCommandHandler>() // Register manualy generic way
                .RegisterDefaultHandler<MyDefaultHandler>();
                //.RegisterDefaultHandler(typeof(MyDefaultHandler));
            //.AutoRegisterHandlers(Assembly.GetEntryAssembly()); // Register automatically

            containerBuilder
                .RegisterType<TestParam>()
                .AsSelf();

            IContainer container = containerBuilder
                .Build();

            var mediator = container.Resolve<IMediator>();

            mediator.Send(new MySimpleEvent());
            MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
            mediator.Send(new MyAnotherEvent());
            MySimpleResponse resp2 = mediator.ExecuteAsync(new MySimpleCommand()).Result;
        }
        //#################################################################



        ////#################################################################
        ///// <summary>
        ///// With any container without integration
        ///// </summary>

        //static void Main(string[] args)
        //{
        //    var containerBuilder = new ContainerBuilder();
        //    var cqrsBuilder = new CqrsBuilder();

        //    cqrsBuilder
        //        //.Register(typeof(MySimpleCommandHandler)) // Register manualy
        //        //.Register(typeof(MySimpleEventHandler));
        //        .AutoRegisterHandlers(Assembly.GetEntryAssembly()) // Register automatically                
        //        .RegisterContainer(new ContainerRegistration(x => containerBuilder.RegisterType(x).AsSelf()));

        //    containerBuilder
        //        .Register(x =>
        //        {
        //            var scope = x.Resolve<ILifetimeScope>();
        //            cqrsBuilder.RegisterResolver(new BasicHandlerResolver(scope.Resolve));
        //            return cqrsBuilder.Build();
        //        })
        //        .As<IMediator>()
        //        .SingleInstance();

        //    containerBuilder
        //        .RegisterType<TestParam>()
        //        .AsSelf();

        //    var container = containerBuilder
        //        .Build();

        //    var mediator = container.Resolve<IMediator>();
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
        //    var cqrsBuilder = new CqrsBuilder();

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
