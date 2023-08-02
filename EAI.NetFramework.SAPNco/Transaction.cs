using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    public class Transaction : IDisposable
    {
        private string _transactionId;
        private string _sessionId;
        private RfcDestination _rfcDestination;
        private bool _isAlive;

        public string TransactionId { get => _transactionId; }
        public bool IsAlive { get => _isAlive; }
        public string SessionId { get => _sessionId; }

        public Transaction(RfcConnection rfcConnection)
        {
            _sessionId = SessionProvider.Instance.CreateSession();
            _transactionId = Guid.NewGuid().ToString();
            _rfcDestination = rfcConnection.GetRfcDestination();

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
                RfcSessionManager.BeginContext(_rfcDestination);

            _isAlive = true;
        }

        public void Commit()
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            var bapiTransactionCommit = _rfcDestination.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
            {
                bapiTransactionCommit.Invoke(_rfcDestination);

                RfcSessionManager.EndContext(_rfcDestination);
            }

            _isAlive = false;
            SessionProvider.Instance.DestroySession(_sessionId);
        }

        public void Rollback()
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            var bapiTransactionRollback = _rfcDestination.Repository.CreateFunction("BAPI_TRANSACTION_ROLLBACK");

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
            {
                bapiTransactionRollback.Invoke(_rfcDestination);

                RfcSessionManager.EndContext(_rfcDestination);
            }

            _isAlive = false;
            SessionProvider.Instance.DestroySession(_sessionId);
        }

        public void InvokeFunction(IRfcFunction function)
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
                function.Invoke(_rfcDestination);
        }

        public void Dispose()
        {
            if(_isAlive) 
                Rollback();
        }
    }
}
