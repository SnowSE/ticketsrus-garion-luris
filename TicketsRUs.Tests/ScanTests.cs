using Moq;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
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

        Mock<ICameraController> mockCamera = new();
        mockCamera.Setup(m => m.GetScanResultsAsync())
            .Returns(Task.FromResult(t.Identifier));

        QRScanner scanner = new(mockService.Object, mockCamera.Object);


        //ACT
        await scanner.DoScanAsync();

        //ASSERT
        mockService.Verify(m => m.GetTicket(It.IsAny<string>()));
        mockService.Verify(m => m.UpdateTicket(It.IsAny<Ticket>()));
        mockCamera.Verify(m => m.GetScanResultsAsync());

        Assert.True(t.Scanned);
    }

    //[Fact]
    //public async void UnsuccessfulScan_DoesNotUpdateDatabasce()
    //{

    //}
}
