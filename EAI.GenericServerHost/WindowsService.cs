using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EAI.GenericServerHost
{
    internal class WindowsService : ServiceBase
    {
        private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected override void OnStart(string[] args)
        {
            Task.Run(() => Program.RunAsync(_cancellationTokenSource.Token));

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _cancellationTokenSource?.Cancel();
            
            base.OnStop();
        }
    }
}
