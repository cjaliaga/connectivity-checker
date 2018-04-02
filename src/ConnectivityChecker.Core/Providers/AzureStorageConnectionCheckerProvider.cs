using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;
using Microsoft.WindowsAzure.Storage;

namespace ConnectivityChecker.Core.Providers
{
    public class AzureStorageConnectionCheckerProvider : IConnectionCheckerProvider
    {
        public bool IsValid(KeyValuePair<string, string> variable)
        {
            return CloudStorageAccount.TryParse(variable.Value, out CloudStorageAccount account);
        }

        public bool IsValid(string providerName)
        {
            return providerName == ProviderNames.AzureStorage.ToString();
        }

        public bool IsValid(ConnectionInfo connection)
        {
            return connection != null && connection.Provider == ProviderNames.AzureStorage;
        }

        public ConnectionInfo GetConnectionInfo(KeyValuePair<string, string> pair)
        {
            return new ConnectionInfo
            {
                Value = pair.Value,
                Provider = ProviderNames.AzureStorage,
                Name = pair.Key
            };
        }

        private async Task<ConnectionStatus> CheckConnectionStatusToQueuesAsync(ConnectionStatus status)
        {
            try
            {
                var connection = CloudStorageAccount.Parse(status.Value);
                var client = connection.CreateCloudQueueClient();
                var data = await client.ListQueuesSegmentedAsync(null);
            }
            catch (Exception ex)
            {
                status.IsWorking = false;
                status.Status = ex.Message;
            }

            return status;
        }

        private async Task<ConnectionStatus> CheckConnectionStatusToBlobAsync(ConnectionStatus status)
        {
            try
            {
                var connection = CloudStorageAccount.Parse(status.Value);
                var client = connection.CreateCloudBlobClient();
                var data = await client.ListContainersSegmentedAsync(null);
            }
            catch (Exception ex)
            {
                status.IsWorking = false;
                status.Status = ex.Message;
            }

            return status;

        }

        public async Task<ConnectionStatus> CheckConnectionStatusAsync(ConnectionInfo info)
        {
            var status = await CheckConnectionStatusToBlobAsync(new ConnectionStatus(info));
            return status;
        }
    }
}
