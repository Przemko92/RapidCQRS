# RapidCQRS

This is free .NET Standard 2.0 library for Command Query Responsibilyty Segregation. You can use this library with .NET Core >= 2.0 and .NET Framework >= 4.61

## Getting Started

These instructions will help you to attach this library to your project

### Installing

Instalation with Nuget

```
Install-Package RapidCqrs
```

For autofac integration use RapidCqrs.Authofac

```
Install-Package RapidCqrs.Autofac
```

## Usage

With autofac integration
```
  static void Main(string[] args)
        {
	    var containerBuilder = new ContainerBuilder();
            containerBuilder
                .AddRapidCqrs()
                .Register(typeof(MySimpleCommandHandler)) // Register manualy
                .Register<MySimpleEventHandler>() // Register manualy generic way
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
		}
```

With any another cointainer
```
  static void Main(string[] args)
        {
	    var containerBuilder = new ContainerBuilder();
            var cqrsBuilder = new CqrsBuilder();

            cqrsBuilder
                //.Register(typeof(MySimpleCommandHandler)) // Register manualy
                //.Register(typeof(MySimpleEventHandler));
                .AutoRegisterHandlers(Assembly.GetEntryAssembly()) // Register automatically                
                .RegisterContainer(new ContainerRegistration(x => containerBuilder.RegisterType(x).AsSelf()));

            containerBuilder
                .Register(x =>
                {
                    var scope = x.Resolve<ILifetimeScope>();
                    cqrsBuilder.RegisterResolver(new BasicHandlerResolver(scope.Resolve));
                    return cqrsBuilder.Build();
                })
                .As<IMediator>()
                .SingleInstance();

            containerBuilder
                .RegisterType<TestParam>()
                .AsSelf();

            var container = containerBuilder
                .Build();

            var mediator = container.Resolve<IMediator>();
            mediator.SendAsync(new MySimpleEvent());
            MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
		}
```

Without any container
```
  static void Main(string[] args)
        {
            var cqrsBuilder = new CqrsBuilder();
            var mediator = cqrsBuilder
                //.Register(typeof(MySimpleCommandHandler)) // Register manualy
                //.Register(typeof(MySimpleEventHandler));
                .AutoRegisterHandlers(Assembly.GetEntryAssembly()) // Register automatically                
                .Build();

            // mediator.SendAsync(new MySimpleEvent()); // No parametreless constructor exception
            MySimpleResponse resp = mediator.Execute(new MySimpleCommand());
        }
```

## Authors

* **Przemysław Grzywa** - [Przemko92](https://github.com/Przemko92)

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details

## Need help with event sourcing?

```
https://msdn.microsoft.com/en-us/library/jj554200.aspx
https://docs.microsoft.com/en-us/nuget/consume-packages/ways-to-install-a-package
```
