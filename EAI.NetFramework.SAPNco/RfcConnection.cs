using Newtonsoft.Json.Linq;
using NCo = SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EAI.SAPNco.Model;

namespace EAI.NetFramework.SAPNco
{
    public class RfcConnection
    {
        private string _connectionString;
        private string _userName;
        private string _password;
        private NCo.RfcConfigParameters _rfcParams;

        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        public void Connect()
        {
            if (_rfcParams != null)
                throw new SapException("Allready connected");

            _rfcParams = new NCo.RfcConfigParameters();

            _rfcParams[NCo.RfcConfigParameters.User] = _userName;
            _rfcParams[NCo.RfcConfigParameters.Password] = _password;

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = _connectionString;

            foreach (var paramKey in builder.Keys.Cast<string>())
                _rfcParams.Add(paramKey.ToUpper(), builder[paramKey]?.ToString());

            if (!_rfcParams.ContainsKey(NCo.RfcConfigParameters.Name))
                _rfcParams[NCo.RfcConfigParameters.Name] = string.Join("-", _rfcParams.Where(p => p.Key != NCo.RfcConfigParameters.Password).OrderBy(p => p.Key).Select(p => p.Value));
        }

        public void Ping()
        {
            var rfcDestination = GetRfcDestination();

            rfcDestination.Ping();
        }

        public NCo.IRfcFunction GetRfcFunction(string name)
        {
            var rfcDestination = GetRfcDestination();

            return rfcDestination.Repository.CreateFunction(name);
        }

        public void InvokeFunction(NCo.IRfcFunction rfcFunction, Transaction transaction = null, bool autoCommit = false)
        {
            if (transaction != null)
            {
                transaction.InvokeFunction(rfcFunction);
                return;
            }

            if(autoCommit)
            {
                using (transaction = new Transaction(this))
                {
                    transaction.InvokeFunction(rfcFunction);

                    transaction.Commit();
                }

                return;
            }

            rfcFunction.Invoke(GetRfcDestination());
        }

        public JProperty InvokeJFunction(JProperty jFunction, Transaction transaction = null, bool autoCommit = false)
        {
            var rfcFunction = JRfc.JsonToRfcFunction(jFunction, GetRfcDestination());

            InvokeFunction(rfcFunction, transaction, autoCommit);

            var jRfcResponse = new JObject();

            JRfc.RfcDataToJson(rfcFunction, jRfcResponse);

            return new JProperty(GetResponsePropertyName(rfcFunction), jRfcResponse);
        }


        public NCo.RfcDestination GetRfcDestination()
        {
            return NCo.RfcDestinationManager.GetDestination(_rfcParams);
        }

        public JObject GetJRfcSchema(string functionName)
        {
            var rfcDestination = GetRfcDestination();

            CheckFunctionExists(rfcDestination, functionName);

            var rfcFunction = rfcDestination.Repository.CreateFunction(functionName);

            var jRfc = new JObject();

            var jRfcFunctionObject = new JObject();
            JRfc.RfcMetadataToJsonSchema(rfcFunction, jRfcFunctionObject, NCo.RfcDirection.IMPORT);

            jRfc.Add(new JProperty(rfcFunction.Metadata.Name, jRfcFunctionObject));

            var jRfcFunctionResponseObject = new JObject();
            JRfc.RfcMetadataToJsonSchema(rfcFunction, jRfcFunctionResponseObject, NCo.RfcDirection.EXPORT);

            jRfc.Add(new JProperty(GetResponsePropertyName(rfcFunction), jRfcFunctionResponseObject));

            return jRfc;
        }

        private static string GetResponsePropertyName(IRfcFunction rfcFunction)
        {
            return $"{rfcFunction.Metadata.Name}_Response";
        }

        public RfcFunctionMetadata GetRfcFunctionMetadata(string functionName)
        {
            var rfcDestination = GetRfcDestination();

            var isFunctionExisting = rfcDestination.Repository.CheckFunctionExists(functionName);
            if (!isFunctionExisting)
                throw new Exception($"Function {functionName} does not exist");

            var rfcFunction = rfcDestination.Repository.CreateFunction(functionName);

            var rfcFunctionMetadata = rfcFunction.Metadata;

            return RfcMetadataFactory.Instance.CreateRfcFunctionMetadata(rfcFunctionMetadata);
        }

        private static void CheckFunctionExists(NCo.RfcDestination rfcDestination, string functionName)
        {
            var isFunctionExisting = rfcDestination.Repository.CheckFunctionExists(functionName);
            if (!isFunctionExisting)
                throw new Exception($"Function {functionName} does not exist");
        }

        public void Disconnect()
        {
            _rfcParams = null;
        }
    }
}
