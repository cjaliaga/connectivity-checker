using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectivityChecker.Contracts.Providers
{
    public interface IConnectionCheckerProvider
    {
        bool IsValid(KeyValuePair<string, string> variable);
        bool IsValid(string providerNamespace);
        bool IsValid(ConnectionInfo connection);
        ConnectionInfo GetConnectionInfo(KeyValuePair<string, string> pair);
        Task<ConnectionStatus> CheckConnectionStatusAsync(ConnectionInfo info);
    }
}
