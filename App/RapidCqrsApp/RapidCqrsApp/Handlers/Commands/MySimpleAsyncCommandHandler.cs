using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RapidCqrs.Helpers.Interfaces;
using RapidCqrsApp.Models.Commands;

namespace RapidCqrsApp.Handlers.Commands
{
    class MySimpleAsyncCommandHandler : IAsyncCommandHandler<MySimpleCommand, MySimpleResponse>
    {
        public async Task<MySimpleResponse> Execute(MySimpleCommand request, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => new MySimpleResponse(), cancellationToken);
        }

        public void Dispose()
        {
        }
    }
}
