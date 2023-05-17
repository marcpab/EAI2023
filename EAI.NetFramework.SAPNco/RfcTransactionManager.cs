using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    public class RfcTransactionManager
    {
        public static RfcTransactionManager Instance = new RfcTransactionManager();


        private Dictionary<string, RfcTransaction> _transactionMap = new Dictionary<string, RfcTransaction>();

        public RfcTransaction CreateTransaction()
        {
            var transaction = new RfcTransaction();
            _transactionMap.Add(transaction.Tid.TID, transaction);

            return transaction;
        }

        public RfcTransaction GetTransaction(string tid)
        {
            try
            {
                return _transactionMap[tid];
            }
            catch(Exception ex) 
            {
                throw new SapException($"Transaction with id {tid} was not found.", ex);
            }
        }

        public void CommitTransaction(RfcTransaction transaction, RfcDestination rfcDestination)
        {
            transaction = GetTransaction(transaction.Tid.TID);

            transaction.Commit(rfcDestination);

            rfcDestination.ConfirmTransactionID(transaction.Tid);

            _transactionMap.Remove(transaction.Tid.TID);
        }

        public void RollbackTransaction(RfcTransaction transaction, RfcDestination rfcDestination)
        {
            transaction = GetTransaction(transaction.Tid.TID);

            rfcDestination.ConfirmTransactionID(transaction.Tid);

            _transactionMap.Remove(transaction.Tid.TID);
        }
    }
}
