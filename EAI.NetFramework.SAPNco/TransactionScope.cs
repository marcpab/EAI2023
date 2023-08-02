using System;

namespace EAI.NetFramework.SAPNco
{
    public class TransactionScope : IDisposable
    {
        private SessionProvider _sessionProvider;

        public TransactionScope(SessionProvider sessionProvider, Transaction transaction)
        {
            _sessionProvider = sessionProvider;
            _sessionProvider.SetCurrentSession(transaction.SessionId);
        }

        public void Dispose()
        {
            _sessionProvider.SetCurrentSession(null);
        }
    }
}
