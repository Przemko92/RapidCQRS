using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RapidCqrs.Builder;
using RapidCqrs.Builder.Interfaces;
using RapidCqrs.Tests.Builder.Models;
using Xunit;

namespace RapidCqrs.Tests.Builder
{
   public class CqrsBuilderTests
    {
        [Fact]
        public void CqrsBuilder_ThrowsException_OnNullRegistration()
        {
            //Arrange
            var builder = new CqrsBuilder();

            //Act Assert
            Assert.Throws<ArgumentNullException>(() => builder.Register(null));
            Assert.Throws<ArgumentNullException>(() => builder.RegisterDefaultHandler(null));
            Assert.Throws<ArgumentNullException>(() => builder.AutoRegisterHandlers(null));
            Assert.Throws<ArgumentNullException>(() => builder.RegisterContainer(null));
            Assert.Throws<ArgumentNullException>(() => builder.RegisterResolver(null));
        }

        [Fact]
        public void CqrsBuilder_ThrowsException_OnInvalidHandlerRegistration()
        {
            //Arrange
            var builder = new CqrsBuilder();

            //Act Assert
            Assert.Throws<InvalidCastException>(() => builder.Register(typeof(InvalidEventHandler)));
            Assert.Throws<InvalidCastException>(() => builder.Register(typeof(InvalidCommandHandler)));
            Assert.Throws<InvalidCastException>(() => builder.RegisterDefaultHandler(typeof(InvalidCommandHandler)));
        }

        [Theory]
        [InlineData(typeof(ValidCommandHandler), typeof(ValidEventHandler), typeof(InvalidCommandHandler), typeof(InvalidEventHandler))]
        public void CqrsBuilder_ThrowsInvalidCast_OnAddingInvalidHanlder(params Type[] handlerTypes)
        {
            //Arrange
            int fireTimes = handlerTypes.Length + 1; // Add default handler for unknown methods
            var builder = new CqrsBuilder();
            var registration = new Mock<IContainerRegistration>();
            registration
                .Setup(x => x.RegisterAction)
                .Returns(x => { });
            
            //Act
            builder.RegisterContainer(registration.Object);
            
            //Assert
            Assert.Throws<InvalidCastException>(() => builder.Register(handlerTypes));
        }

        [Theory]
        [InlineData(typeof(ValidEventHandler))]
        [InlineData(typeof(ValidCommandHandler))]
        [InlineData(typeof(ValidCommandHandler), typeof(ValidEventHandler))]
        public void CqrsBuilder_FiresContainerRegistry_OnContainerRegistration(params Type[] handlerTypes)
        {
            //Arrange
            int fireTimes = handlerTypes.Length + 1; // Add default handler for unknown methods
            var builder = new CqrsBuilder();
            var registration = new Mock<IContainerRegistration>();
            registration
                .Setup(x => x.RegisterAction)
                .Returns(x => { });

            //Act
            builder.Register(handlerTypes);
            builder.RegisterContainer(registration.Object);

            //Assert
            registration.Verify(x => x.RegisterAction, Times.Exactly(fireTimes));
        }
    }
}
