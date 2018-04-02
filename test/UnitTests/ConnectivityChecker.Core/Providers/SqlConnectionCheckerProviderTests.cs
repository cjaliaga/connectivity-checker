using ConnectivityChecker.Core.Providers;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using ConnectivityChecker.Contracts.Providers;
using ConnectivityChecker.Contracts;

namespace UnitTests.ConnectivityChecker.Core.Providers
{
    public class sql_connection_checker_should
    {
        [Theory]
        [MemberData(nameof(GenerateKeyValuePair))]
        public void validate_keyvaluepair_connection_string(KeyValuePair<string, string> variable, bool expected)
        {
            var provider = new SqlConnectionCheckerProvider();
            provider.IsValid(variable).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GenerateProviderNamespace))]
        public void validate_provider_namespace_connection_string(string variable, bool expected)
        {
            var provider = new SqlConnectionCheckerProvider();
            provider.IsValid(variable).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GenerateConnectionInfo))]
        public void validate_connection_info(ConnectionInfo info, bool expected)
        {
            var provider = new SqlConnectionCheckerProvider();
            provider.IsValid(info).Should().Be(expected);
        }

        public static IEnumerable<object[]> GenerateKeyValuePair()
        {
            yield return new object[] { KeyValuePair.Create("DB", @"Server=tcp:[serverName].database.windows.net;Database=myDataBase;
            User ID=[LoginForDb]@[serverName]; Password = myPassword; Trusted_Connection = False;
            Encrypt = True;"), true };
            yield return new object[] { KeyValuePair.Create("Storage", @"DefaultEndpointsProtocol=https;AccountName=popcornworld;AccountKey=Riverside==;EndpointSuffix=core.windows.net"), false };
            yield return new object[] { KeyValuePair.Create<string,string>("Null", null), false };
            yield return new object[] { KeyValuePair.Create("Empty", string.Empty), false };
        }

        public static IEnumerable<object[]> GenerateProviderNamespace()
        {
            yield return new object[] { "System.Data.SqlClient", true };
            yield return new object[] { "System.PopCorn.Movie", false };
            yield return new object[] { null, false };
        }

        public static IEnumerable<object[]> GenerateConnectionInfo()
        {
            yield return new object[] { new ConnectionInfo { Provider = ProviderNames.SqlServer}, true };
            yield return new object[] { new ConnectionInfo { Provider = ProviderNames.MySql }, false };
            yield return new object[] { null, false };
        }
    }
}
