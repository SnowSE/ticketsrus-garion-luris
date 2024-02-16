using Moq;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.Maui.Components;
using TicketsRUs.Maui.Controllers;

namespace TicketsRUs.Tests;

public class ScanTests
{
    [Fact]
    public void EnvironmentSetUp()
    {
        Assert.True(true);
    }

    [Fact]
    public async void SuccessfulScan_UpdatesDatabase()
    {
        //ARRANGE
        Ticket t = new Ticket()
        {
            Id = 0,
            EventId = 0,
            Scanned = false,
            Identifier = "123456789"
        };

        Mock<ITicketService> mockService = new();
        mockService.Setup(m => m.GetTicket(It.IsAny<string>()))
            .Returns(Task.FromResult(t));
        mockService.Setup(m => m.UpdateTicket(It.IsAny<Ticket>()))
            .Callback(() => t.Scanned = true);

        MauiTicketController controller = new(mockService.Object);

        Mock<QRScanner> mockScanner = new(controller);
        mockScanner.Setup(m => m.DoScanAsync())
            .Callback(async () => { 
                await mockScanner.Object.GetScanResultsAsync();
                await controller.UpdateTicket(await controller.GetTicket(t.Identifier));
            });
        mockScanner.Setup(m => m.GetScanResultsAsync())
            .Returns(Task.FromResult(t.Identifier));


        //ACT
        await mockScanner.Object.DoScanAsync();

        //ASSERT
        mockService.Verify(m => m.GetTicket(It.IsAny<string>()));
        mockService.Verify(m => m.UpdateTicket(It.IsAny<Ticket>()));

        mockScanner.Verify(m => m.DoScanAsync());
        mockScanner.Verify(m => m.GetScanResultsAsync());
    }
}
