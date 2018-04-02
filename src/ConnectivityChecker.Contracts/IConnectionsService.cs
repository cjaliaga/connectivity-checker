using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectivityChecker.Contracts
{
    public interface IConnectionsService
    {
        Task<ConnectionStatus> CheckConnectionStatus(ConnectionInfo connection);
        Task<IEnumerable<ConnectionStatus>> CheckConnectionStatus(IEnumerable<ConnectionInfo> connections);
        IEnumerable<ConnectionInfo> FindConnectionsStringForAzureWebApps(IEnumerable<KeyValuePair<string, string>> variables);
        IEnumerable<ConnectionInfo> FindConnectionStrings(IEnumerable<KeyValuePair<string, string>> variables);
        IEnumerable<ConnectionInfo> FindConnectionStrings(IEnumerable<KeyValuePair<string, string>> variables, string prefix);
        IEnumerable<ConnectionInfo> FindConnectionStringsWithProvider(IEnumerable<KeyValuePair<string, string>> variables);
        IEnumerable<ConnectionInfo> FindConnectionStringsWithProvider(IEnumerable<KeyValuePair<string, string>> variables, string prefix);
    }
}