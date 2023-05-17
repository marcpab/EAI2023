using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    public class RfcConnection
    {
        private string _connectionString;
        private string _userName;
        private string _password;
        private RfcConfigParameters _rfcParams;

        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        public void Connect()
        {
            if (_rfcParams != null)
                throw new SapException("Alleady connected");

            _rfcParams = new RfcConfigParameters();

            _rfcParams[RfcConfigParameters.User] = _userName;
            _rfcParams[RfcConfigParameters.Password] = _password;

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = _connectionString;

            foreach (var paramKey in builder.Keys.Cast<string>())
                _rfcParams.Add(paramKey, builder[paramKey]?.ToString());

            Ping();
        }

        public void Ping()
        {
            var rfcDestination = GetRfcDestination();

            rfcDestination.Ping();
        }

        public IRfcFunction GetRfcFunction(string name)
        {
            var rfcDestination = GetRfcDestination();

            return rfcDestination.Repository.CreateFunction(name);
        }


        public RfcDestination GetRfcDestination()
        {
            return RfcDestinationManager.GetDestination(_rfcParams);
        }

        public void Disconnect()
        {
        }
    }
}
