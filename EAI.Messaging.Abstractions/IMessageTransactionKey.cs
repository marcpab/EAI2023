using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Messaging.Abstractions
{
    public interface IMessageTransactionKey
    {
        string TransactionKey { get; }
    }
}
