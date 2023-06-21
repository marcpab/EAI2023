using SAP.Middleware.Connector;
using System;
using System.Threading;

namespace EAI.NetFramework.SAPNco
{
    public class SessionProvider : ISessionProvider
    {
        private static SessionProvider _instance = new SessionProvider();

        public static SessionProvider Instance { get { return _instance; } }

        static SessionProvider()
        {
            RfcSessionManager.RegisterSessionProvider(Instance);
        }

        private ThreadLocal<Transaction> _currentTransaction = new ThreadLocal<Transaction>();

        

        internal void SetCurrentTransaction(Transaction transaction)
        {
            _currentTransaction.Value = transaction;
        }


        public event RfcSessionManager.SessionChangeHandler SessionChanged;

        public void ActivateSession(string sessionID)
        {
        }

        public bool ChangeEventsSupported()
        {
            return false;
        }

        public void ContextFinished()
        {
        }

        public void ContextStarted()
        {
        }

        public string CreateSession()
        {
            var currentTransaction = _currentTransaction.Value;

            if (currentTransaction == null)
                throw new InvalidOperationException("No current transaction");

            return currentTransaction.TransactionId;
        }

        public void DestroySession(string sessionID)
        {
        }

        public string GetCurrentSession()
        {
            var currentTransaction = _currentTransaction.Value;

            if (currentTransaction == null)
                throw new InvalidOperationException("No current transaction");

            return currentTransaction.TransactionId;
        }

        public bool IsAlive(string sessionID)
        {
            return true;
        }

        public void PassivateSession(string sessionID)
        {
        }
    }

}
