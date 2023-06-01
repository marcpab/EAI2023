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
        private RfcDestination _rfcDestination;
        private bool _isAlive;

        public string TransactionId { get => _transactionId; }
        public bool IsAlive { get => _isAlive; }

        public Transaction(RfcConnection rfcConnection)
        {
            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
            {
                _transactionId = Guid.NewGuid().ToString();
                _rfcDestination = rfcConnection.GetRfcDestination();
                RfcSessionManager.BeginContext(_rfcDestination);
                _isAlive = true;
            }
        }

        public void Commit()
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
            {
                var bapiTransactionCommit = _rfcDestination.Repository.CreateFunction("BAPI_TRANSACTION_COMMIT");

                bapiTransactionCommit.Invoke(_rfcDestination);

                RfcSessionManager.EndContext(_rfcDestination);
                _isAlive = false;
            }
        }

        public void Rollback()
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            using (var transactionScope = new TransactionScope(SessionProvider.Instance, this))
            {
                var bapiTransactionRollback = _rfcDestination.Repository.CreateFunction("BAPI_TRANSACTION_ROLLBACK");

                bapiTransactionRollback.Invoke(_rfcDestination);

                RfcSessionManager.EndContext(_rfcDestination);
                _isAlive = false;
            }
        }

        public void InvokeFunction(IRfcFunction function)
        {
            if (!_isAlive)
                throw new InvalidOperationException("Transaction already completed.");

            function.Invoke(_rfcDestination);
        }

        public void Dispose()
        {
            if(_isAlive) 
                Rollback();
        }
    }
}
