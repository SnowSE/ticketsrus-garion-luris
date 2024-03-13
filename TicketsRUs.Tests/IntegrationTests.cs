using Moq;

using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.Tests;

public class IntegrationTests : IClassFixture<TestFactory>
{
    public HttpClient client { get; set; }
    public IntegrationTests(TestFactory Factory)
    {
        client = Factory.CreateDefaultClient();
    }

    [Fact]
    public void CanPassATest()
    {
        Assert.Equal(1, 1);
    }
}