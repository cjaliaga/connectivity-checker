using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;
using MySql.Data.MySqlClient;

namespace ConnectivityChecker.Core.Providers
{
    public class MySqlConnectionCheckerProvider : IConnectionCheckerProvider
    {
        private const string ProviderNamespace = "MySql.Data.MySqlClient";

        public bool IsValid(KeyValuePair<string, string> variable)
        {
            if (string.IsNullOrEmpty(variable.Value))
            {
                return false;
            }

            var value = variable.Value.ToLower();
            return value.Contains("port=3306") || value.Contains("mysql.database.azure.com") ||
                   value.Contains(":3306") || value.Contains("uid=");
        }

        public bool IsValid(string providerNamespace)
        {
            return providerNamespace == ProviderNamespace;
        }

        public bool IsValid(ConnectionInfo connection)
        {
            return connection.Provider == ProviderNames.MySql;
        }

        public ConnectionInfo GetConnectionInfo(KeyValuePair<string, string> pair)
        {
            return new ConnectionInfo
            {
                Provider = ProviderNames.MySql,
                Name = pair.Key,
                Value = pair.Value
            };
        }

        public async Task<ConnectionStatus> CheckConnectionStatusAsync(ConnectionInfo info)
        {
            var status = new ConnectionStatus(info);

            try
            {
                using (var connection = new MySqlConnection(info.Value))
                {
                    await connection.OpenAsync();
                }
            }
            catch (Exception ex)
            {
                status.IsWorking = false;
                status.Status = ex.Message;
            }

            return status;
        }
    }
}
