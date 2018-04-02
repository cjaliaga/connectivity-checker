using ConnectivityChecker.Contracts;
using ConnectivityChecker.Contracts.Providers;
using ConnectivityChecker.Core;
using ConnectivityChecker.Core.Providers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConnectivityCheckerServiceCollectionExtensions
    {
        public static IServiceCollection AddConnectivityChecker(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionCheckerProvider, MySqlConnectionCheckerProvider>();
            services.AddSingleton<IConnectionCheckerProvider, SqlConnectionCheckerProvider>();
            services.AddSingleton<IConnectionCheckerProvider, AzureStorageConnectionCheckerProvider>();
            services.TryAddSingleton<IConnectionsService,ConnectionsService>();

            return services;
        }
    }
}
