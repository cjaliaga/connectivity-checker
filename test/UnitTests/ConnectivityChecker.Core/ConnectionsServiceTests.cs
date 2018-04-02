using Xunit;
using FluentAssertions;
using ConnectivityChecker.Core;
using System.Collections.Generic;
using ConnectivityChecker.Contracts.Providers;
using ConnectivityChecker.Contracts;
using System.Threading.Tasks;
using System;
using Moq;

namespace UnitTests.ConnectivityChecker.Core
{
    public class connections_service_should
    {
        [Fact]
        public async Task check_connection_status_throws_argument_null_exception()
        {
            var service = new ConnectionsService(new List<IConnectionCheckerProvider>());
            ConnectionInfo info = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.CheckConnectionStatus(info));
        }

        [Theory]
        [MemberData(nameof(GenerateVariables))]
        public void find_connection_info_from_variables_when_theres_providers(IEnumerable<KeyValuePair<string, string>> variables, int expected)
        {
            var provider1 = new Mock<IConnectionCheckerProvider>();
            provider1.Setup(p => p.IsValid(It.IsAny<KeyValuePair<string, string>>())).Returns((KeyValuePair<string, string> pair) => pair.Value == "PROVIDER1");
            provider1.Setup(p => p.GetConnectionInfo(It.IsAny<KeyValuePair<string, string>>())).Returns((KeyValuePair<string, string> pair) => new ConnectionInfo {Name = pair.Key, Value = pair.Value, Provider = ProviderNames.AzureStorage });

            var provider2 = new Mock<IConnectionCheckerProvider>();
            provider2.Setup(p => p.IsValid(It.IsAny<KeyValuePair<string, string>>())).Returns((KeyValuePair<string, string> pair) => pair.Value == "PROVIDER2");
            provider2.Setup(p => p.GetConnectionInfo(It.IsAny<KeyValuePair<string, string>>())).Returns((KeyValuePair<string, string> pair) => new ConnectionInfo { Name = pair.Key, Value = pair.Value, Provider = ProviderNames.MySql });

            var providers = new List<IConnectionCheckerProvider>
            {
                provider1.Object,
                provider2.Object
            };

            var service = new ConnectionsService(providers);

            service.FindConnectionStrings(variables).Should().HaveCount(expected);
        }

        public static IEnumerable<object[]> GenerateVariables()
        {
            var variables = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ONE","PROVIDER1"),
                new KeyValuePair<string, string>("TWO","PROVIDER2"),

            };

            yield return new object[] { variables, 2 };

            variables = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ONE","PROVIDER1"),
                new KeyValuePair<string, string>("TWO","PROVIDER4"),

            };

            yield return new object[] { variables, 1 };
        }
    }
}
