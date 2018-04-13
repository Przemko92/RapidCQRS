using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Helpers;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrs.Models.Interfaces;
using RapidCqrs.Tests.Builder.Models.Commands;
using Xunit;

namespace RapidCqrs.Tests.Helpers
{
    public class MediatorTests
    {
        [Fact]
        public void Mediator_FiresMethodOnEventHandler_OnNewEvent()
        {
            //Arrange
            var handler = new Mock<IEventHandler<TestEvent>>();
            handler
                .Setup(x => x.Publish(It.IsAny<TestEvent>()));

            var handlersFactory = new Mock<IHandlersFactory>();

            handlersFactory
                .Setup(x => x.GetHandler(typeof(TestEvent)))
                .Returns(() => handler.Object);

            var mediator = new Mediator(handlersFactory.Object);

            //Act 
            mediator.Send(new TestEvent());

            //Assert
            handler.Verify(x => x.Publish(It.IsAny<TestEvent>()), Times.Once);
        }

        [Fact]
        public void Mediator_FiresMethodOnCommandHandler_OnNewCommand()
        {
            //Arrange
            var handler = new Mock<ICommandHandler<TestCommand, object>>();
            handler
                .Setup(x => x.Execute(It.IsAny<TestCommand>()));

            var handlersFactory = new Mock<IHandlersFactory>();

            handlersFactory
                .Setup(x => x.GetHandler(typeof(TestCommand)))
                .Returns(() => handler.Object);

            var mediator = new Mediator(handlersFactory.Object);

            //Act 
            mediator.Execute(new TestCommand());

            //Assert
            handler.Verify(x => x.Execute(It.IsAny<TestCommand>()), Times.Once);
        }
    }
}
