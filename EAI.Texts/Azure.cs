namespace EAI.Texts
{
    public static class Azure
    {
        public const string WebJobsStorage = "AzureWebJobsStorage";
        public const string ServiceConfigurationContainer = "eai-configuration";
        public const string ServiceConfigurationBlob = "service-configuration.json";

        public static string SrvCfgKey_CDSEndpoint(string mode) 
            => $"cds{Templates.TrimAll(mode)?.ToUpper()}";
        public static readonly string SrvCfg_EndpointUrl = "ServiceUrl";
        public static readonly string SrvCfg_TennantId = "TennantId";
        public static readonly string SrvCfg_ClientId = "ClientId";
        public static readonly string SrvCfg_ClientSecret = "ClientSecret";

        public static readonly string HeaderOriginXId = "originxid";
        public static readonly string XWorkflowName = "x-ms-workflow-name";
        public static readonly string XCorrelationId = "x-ms-correlation-id";
        public static readonly string XSiteDeployment = "x-site-deployment-id";
        public static readonly string UserAgent = "user-agent";
        public static readonly string ClientIP = "CLIENT-IP";
        public static readonly string XWAWS = "X-WAWS-Unencoded-URL";
    }
}
