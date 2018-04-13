using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Helpers.Models;
using RapidCqrs.Tests.Builder.Models;
using RapidCqrs.Tests.Builder.Models.Commands;
using Xunit;

namespace RapidCqrs.Tests.Builder
{
    public class HandlerFactoryTests
    {
        [Theory]
        [InlineData(typeof(TestCommand), typeof(ValidCommandHandler))]
        [InlineData(typeof(TestEvent), typeof(ValidEventHandler))]
        public void HandlerFactory_ReturnsValidHandler_OnGetHandler(Type eventType, Type handlerType)
        {
            //Arrange
            var handlersDictionary = new Dictionary<Type, Func<IHandler>>();
            handlersDictionary.Add(typeof(TestCommand), () => new ValidCommandHandler());
            handlersDictionary.Add(typeof(TestEvent), () => new ValidEventHandler());
            var handlerFactory = new HandlersFactory(handlersDictionary, null);

            //Act
            var handler = handlerFactory.GetHandler(eventType);

            //Assert
            Assert.IsAssignableFrom(handlerType, handler);
        }

        [Fact]
        public void HandlerFactory_ReturnsDefaultHandler_OnGetHandlerWithNotFoundType()
        {
            //Arrange
            var handlersDictionary = new Dictionary<Type, Func<IHandler>>();
            var handlerFactory = new HandlersFactory(handlersDictionary, () => new DefaultHandler());

            //Act
            var handler = handlerFactory.GetHandler(typeof(TestEvent));
            var handler2 = handlerFactory.GetHandler(typeof(TestCommand));

            //Assert
            Assert.IsAssignableFrom<DefaultHandler>(handler);
            Assert.IsAssignableFrom<DefaultHandler>(handler2);
        }

        [Fact]
        public void HandlerFactory_ThrowsInvalidDataException_OnGetHandlerWithNotFoundTypeAndNotRegisteredDefault()
        {
            //Arrange
            var handlersDictionary = new Dictionary<Type, Func<IHandler>>();
            var handlerFactory = new HandlersFactory(handlersDictionary, null);

            //Act Assert
            Assert.Throws<InvalidDataException>(() => handlerFactory.GetHandler(typeof(TestEvent)));
            Assert.Throws<InvalidDataException>(() => handlerFactory.GetHandler(typeof(TestCommand)));
        }
    }
}
