using ConnectivityChecker.Contracts;
using ConnectivityChecker.Core;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace UnitTests.ConnectivityChecker.Core
{
    public class service_collection_extensions_AddConnectivityChecker_should
    {
        private readonly IServiceProvider _serviceProvider;

        public service_collection_extensions_AddConnectivityChecker_should()
        {
            _serviceProvider = WebHost.CreateDefaultBuilder()
                .UseStartup<DefaultStartup>()
                .Build()
                .Services;
        }

        [Fact]
        public void register_connectivity_checker_service()
        {
            _serviceProvider.GetService<IConnectionsService>()
                .Should()
                .BeOfType<ConnectionsService>();
        }

        class DefaultStartup
        {
            public void ConfigureServices(IServiceCollection services) => services.AddConnectivityChecker();

            public void Configure(IApplicationBuilder app)
            {
            }
        }
    }
}
