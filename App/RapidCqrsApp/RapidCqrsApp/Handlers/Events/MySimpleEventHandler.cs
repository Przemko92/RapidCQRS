using RapidCqrs.Helpers.Interfaces;
using RapidCqrsApp.Models;
using RapidCqrsApp.Models.Events;

namespace RapidCqrsApp.Handlers.Events
{
    public class MySimpleEventHandler : IEventHandler<MySimpleEvent>
    {
        public MySimpleEventHandler(TestParam aa)
        {
            
        }

        public void Publish(MySimpleEvent @event)
        {
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
