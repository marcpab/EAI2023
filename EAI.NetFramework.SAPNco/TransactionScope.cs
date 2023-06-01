using System;

namespace EAI.NetFramework.SAPNco
{
    public class TransactionScope : IDisposable
    {
        private SessionProvider _sessionProvider;

        public TransactionScope(SessionProvider sessionProvider, Transaction transaction)
        {
            _sessionProvider = sessionProvider;
            _sessionProvider.SetCurrentTransaction(transaction);
        }

        public void Dispose()
        {
            _sessionProvider.SetCurrentTransaction(null);
        }
    }

}
