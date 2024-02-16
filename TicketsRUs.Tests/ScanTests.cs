using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.Maui.Components;

namespace TicketsRUs.Tests;

public class ScanTests
{
    [Fact]
    public void EnvironmentSetUp()
    {
        Assert.True(true);
    }

    [Fact]
    public void SuccessfulScan_UpdatesDatabase()
    {
        // ARRANGE
        Mock<QRScanner> mockScanner = new();
        mockScanner.Setup(m => m.DoScanAsync()).Callback(async () => await mockScanner.Object.GetScanResultsAsync());
        mockScanner.Setup(m => m.GetScanResultsAsync());

        // ACT


        // ASSERT

    }
}
