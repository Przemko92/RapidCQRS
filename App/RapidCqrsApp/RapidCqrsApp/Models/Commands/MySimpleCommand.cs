using RapidCqrs.Models.Interfaces;

namespace RapidCqrsApp.Models.Commands
{
    public class MySimpleCommand : ICommand<MySimpleResponse>
    {
        public string First { get; set; }
        public int Second { get; set; }
    }
}
