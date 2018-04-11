using RapidCqrs.Helpers.Interfaces;
using RapidCqrsApp.Models.Commands;

namespace RapidCqrsApp.Handlers.Commands
{
    public class MySimpleCommandHandler : ICommandHandler<MySimpleCommand, MySimpleResponse>
    {
        public MySimpleResponse Execute(MySimpleCommand request)
        {
            return null;
            //throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
