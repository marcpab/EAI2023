using Newtonsoft.Json.Linq;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    public class JRfcContext
    {
        private RfcDestination _rfcDestination;
        private RfcTransaction _rfcTransaction;
        private bool _rfcAutoCommit;

        public void RunJRfcRequest(JToken jRfcRequestMessage, RfcConnection rfcConnection)
        {
            _rfcDestination = rfcConnection.GetRfcDestination();
            _rfcAutoCommit = false;

            {
                var jObject = jRfcRequestMessage as JObject;
                if (jObject != null)
                    RunJRfcRequest(jObject);
            }

            {
                var jArray = jRfcRequestMessage as JArray;
                if (jArray != null)
                    foreach (var jElement in jArray)
                    {
                        var jObject = jElement as JObject;
                        if (jObject != null)
                            RunJRfcRequest(jObject);

                    }
            }
        }

        public void RunJRfcRequest(JObject jRfcRequestMessage)
        {
            foreach (var jRfcRequest in jRfcRequestMessage.Properties().ToArray())
                switch(jRfcRequest.Name)
                {
                    case "BeginTransaction":
                        _rfcTransaction = RfcTransactionManager.Instance.CreateTransaction();

                        jRfcRequest.Value = new JObject(new JProperty("TransactionId", new JValue(_rfcTransaction.Tid.TID)));

                        _rfcAutoCommit = false;

                        break;

                    case "TransactionId":
                        
                        var jTid = jRfcRequest.Value as JValue;
                        
                        _rfcTransaction = RfcTransactionManager.Instance.GetTransaction(jTid.Value.ToString());
                        _rfcAutoCommit = false;

                        break;

                    case "CommitTransaction":

                        RfcTransactionManager.Instance.CommitTransaction(_rfcTransaction, _rfcDestination);
                        _rfcTransaction = null;

                        break;

                    case "RollbackTransaction":

                        RfcTransactionManager.Instance.RollbackTransaction(_rfcTransaction, _rfcDestination);
                        _rfcTransaction = null;

                        break;

                    case "AutoCommit":

                        var jAutoCommit = jRfcRequest.Value as JValue;

                        _rfcAutoCommit = jAutoCommit.Value<bool>();

                        break;

                    default:
                        var rfcFunction = JRfc.JsonToRfcFunction(jRfcRequest, _rfcDestination);

                        if(_rfcAutoCommit)
                            _rfcTransaction = RfcTransactionManager.Instance.CreateTransaction();

                        if (_rfcTransaction != null)
                            _rfcTransaction.AddFunction(rfcFunction);
                        else
                        {
                            rfcFunction.Invoke(_rfcDestination);

                            var jRfcResponse = new JObject();

                            JRfc.RfcDataToJson(rfcFunction, jRfcResponse);

                            jRfcRequest.AddAfterSelf(new JProperty($"{jRfcRequest.Name}_Response", jRfcResponse));
                        }

                        if (_rfcAutoCommit)
                            _rfcTransaction.Commit(_rfcDestination);

                        break;
                }
        }
    }
}
