using ConnectivityChecker.Contracts.Providers;

namespace ConnectivityChecker.Contracts
{
    public class ConnectionInfo
    {
        public ConnectionInfo()
        {
            
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public ProviderNames Provider { get; set; }
    }
}