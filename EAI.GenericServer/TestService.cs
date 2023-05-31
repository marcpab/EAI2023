using EAI.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServer
{
    internal class TestService : IService
    {
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var i = 0;

            while(!cancellationToken.IsCancellationRequested)
                try
                {
                    Console.WriteLine($"{nameof(TestService)} {i}");

                    await Task.Delay(1000, cancellationToken);

                    i++;
                }
                catch(TaskCanceledException) 
                {
                    
                }

        }
    }
}
