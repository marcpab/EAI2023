using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServer
{
    public interface IService
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
