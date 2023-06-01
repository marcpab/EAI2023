using EAI.PipeMessaging.SAPNcoService;

namespace EAI.GenericServer.SAPNcoService
{
    public class SapSystem
    {
        private string _name;
        private string _connectionString;
        private string _userName;
        private string _password;

        private IRfcCallService _rfcCallService;

        public string Name { get => _name; set => _name = value; }
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        internal async Task ConnectAsync(string pipeName)
        {
            try
            {

                _rfcCallService = await RfcCallServiceStub.CreateObjectAsync(pipeName);

                await _rfcCallService.ConnectAsync(
                    _connectionString,
                    _userName,
                    _password);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal Task<string> RunJRfcRequestAsync(string jRfcRequestMessage)
        {
            return _rfcCallService.RunJRfcRequestAsync(jRfcRequestMessage);
        }

        internal async Task DisconnectAsync()
        {
            await _rfcCallService.DisconnectAsync();

            (_rfcCallService as IDisposable)?.Dispose();
        }

        internal Task<string> GetJRfcSchemaAsync(string functionName)
        {
            return _rfcCallService.GetJRfcSchemaAsync(functionName);
        }
    }
}