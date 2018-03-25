using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;

namespace ConnectivityChecker.Core.Providers
{
    public class SqlConnectionCheckerProvider : IConnectionCheckerProvider
    {
        private const string ProviderNamespace = "System.Data.SqlClient";

        public bool IsValid(KeyValuePair<string, string> variable)
        {
            if (string.IsNullOrEmpty(variable.Value))
            {
                return false;
            }

            var result = true;
            try
            {
                using (var connection = new SqlConnection(variable.Value))
                {
                    
                }
            }
            catch (ArgumentException)
            {
                result = false;
            }

            return result;
        }

        public bool IsValid(string providerNamespace)
        {
            return providerNamespace == ProviderNamespace;
        }

        public ConnectionInfo GetConnectionInfo(KeyValuePair<string, string> pair)
        {
            return new ConnectionInfo
            {
                Name = pair.Key,
                Value = pair.Value,
                Provider = ProviderNames.SqlServer
            };
        }

        public async Task<ConnectionStatus> CheckConnectionStatusAsync(ConnectionInfo info)
        {
            var status = new ConnectionStatus(info);

            try
            {
                using (var connection = new SqlConnection(info.Value))
                {
                    await connection.OpenAsync();
                }
            }
            catch (ArgumentException exception)
            {
                status.IsWorking = false;
                status.Status = exception.Message;
            }
            catch (SqlException exception)
            {
                status.IsWorking = false;
                status.Status = exception.Message;
            }

            return status;
        }

        public bool IsValid(ConnectionInfo connection)
        {
            return connection.Provider == ProviderNames.SqlServer;
        }
    }
}