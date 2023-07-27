using NCo = SAP.Middleware.Connector;
using System.Collections.Generic;
using SAP.Middleware.Connector;

namespace EAI.NetFramework.SAPNco
{
    internal class RfcTransactionIdHandler : NCo.ITransactionIDHandler
    {
        private List<string> _transactionIds = new List<string>();

        public bool CheckTransactionID(NCo.RfcServerContextInfo ctx, NCo.RfcTID tid)
        {
            lock (_transactionIds)
            {
                if (_transactionIds.Contains(tid.TID))
                    return false;

                _transactionIds.Add(tid.TID);

                return true;
            }
        }

        public void Commit(NCo.RfcServerContextInfo ctx, NCo.RfcTID tid)
        {
        }

        public void ConfirmTransactionID(NCo.RfcServerContextInfo ctx, NCo.RfcTID tid)
        {
            lock (_transactionIds)
                if (_transactionIds.Contains(tid.TID))
                    _transactionIds.Remove(tid.TID);
        }

        public void Rollback(NCo.RfcServerContextInfo ctx, NCo.RfcTID tid)
        {
            lock (_transactionIds)
                if (_transactionIds.Contains(tid.TID))
                    _transactionIds.Remove(tid.TID);
        }
    }

}
