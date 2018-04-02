using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;

namespace ConnectivityChecker.Core
{
    public class ConnectionsService : IConnectionsService
    {
        private readonly IEnumerable<IConnectionCheckerProvider> _providers;

        public ConnectionsService(IEnumerable<IConnectionCheckerProvider> providers)
        {
            _providers = providers;
        }

        public IEnumerable<ConnectionInfo> FindConnectionStrings(IEnumerable<KeyValuePair<string, string>> variables)
        {
            foreach (var pair in variables)
            {
                var provider = _providers.FirstOrDefault(p => p.IsValid(pair));
                if (provider != null)
                {
                    yield return provider.GetConnectionInfo(pair);
                }
            }
        }

        public IEnumerable<ConnectionInfo> FindConnectionStrings(IEnumerable<KeyValuePair<string, string>> variables, string prefix)
        {
            return FindConnectionStrings(variables.Select(k => new KeyValuePair<string, string>(k.Key.Replace(prefix, ""), k.Value)));
        }

        public IEnumerable<ConnectionInfo> FindConnectionStringsWithProvider(IEnumerable<KeyValuePair<string, string>> variables)
        {
            var values = variables.Where(k => !k.Key.Contains("_ProviderName") && !string.IsNullOrEmpty(k.Value));

            foreach (var value in values)
            {
                var providerValue = variables.FirstOrDefault(k => k.Key == $"{value.Key}_ProviderName");
                IConnectionCheckerProvider provider;

                if (!providerValue.Equals(default(KeyValuePair<string, string>)))
                {
                    provider = _providers.FirstOrDefault(p => p.IsValid(providerValue.Value));
                }
                else
                {
                    provider = _providers.FirstOrDefault(p => p.IsValid(value));
                }

                if (provider != null)
                {
                    yield return provider.GetConnectionInfo(value);
                }
            }

        }

        public IEnumerable<ConnectionInfo> FindConnectionStringsWithProvider(IEnumerable<KeyValuePair<string, string>> variables, string prefix)
        {
            return FindConnectionStringsWithProvider(variables.Select(k => new KeyValuePair<string, string>(k.Key.Replace(prefix, ""), k.Value)));
        }

        public async Task<ConnectionStatus> CheckConnectionStatus(ConnectionInfo connection)
        {
            if(connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var provider = _providers.FirstOrDefault(p => p.IsValid(connection));
            if (provider != null)
            {
                var status = await provider.CheckConnectionStatusAsync(connection);
                return status;
            }

            return null;
        }

        public async Task<IEnumerable<ConnectionStatus>> CheckConnectionStatus(IEnumerable<ConnectionInfo> connections)
        {
            var statuses = new List<Task<ConnectionStatus>>();

            foreach (var connection in connections)
            {
                statuses.Add(CheckConnectionStatus(connection));
            }

            return await Task.WhenAll(statuses);
        }

        public IEnumerable<ConnectionInfo> FindConnectionsStringForAzureWebApps(IEnumerable<KeyValuePair<string, string>> variables)
        {
            var connectionStrings = variables.Where(kv => kv.Key.StartsWith("ConnectionStrings:"));
            var connections = FindConnectionStringsWithProvider(connectionStrings, "ConnectionStrings:");

            var appSettings = variables.Where(kv => kv.Key.StartsWith("APPSETTING_"));
            connections = connections.Union(FindConnectionStrings(appSettings, "APPSETTING_"));

            return connections;
        }
    }
}
