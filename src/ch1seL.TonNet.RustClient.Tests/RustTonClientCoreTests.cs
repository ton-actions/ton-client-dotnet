using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ch1seL.TonClientDotnet.UnitTests
{
    public class RustTonClientCoreTests
    {
        [Fact]
        public void CreatingClientInitNotThrowException()
        {
            var act = new Action(() =>
            {
                using RustTonClientCore client = TestsHelpers.CreateTonClient();
            });

            act.Should().NotThrow();
        }

        [Fact]
        public async Task FactorizeReturnsCorrectOutput()
        {
            using RustTonClientCore client = TestsHelpers.CreateTonClient();
            const string method = "crypto.factorize";
            var parameters = new
            {
                    composite = "6FFAD60473B3"
            };
            var response = await client.Request(method, JsonSerializer.Serialize(parameters, RustTonClientCore.JsonSerializerOptions));

            response.Should().Be("{\"factors\":[\"3\",\"25539CAC2691\"]}");
        }

        [Fact]
        public async Task VersionRequestResponseWithNotEmptyResultAndNullError()
        {
            using RustTonClientCore client = TestsHelpers.CreateTonClient();

            var response = await client.Request("client.version", null);

            response.Should().Be("{\"version\":\"1.1.1\"}");
        }
    }
}