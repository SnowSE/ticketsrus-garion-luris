using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.ClassLib;

public class QRScanner
{
    public string ScanResult { get; private set; } = "";
    public bool SuccessfulScan { get; private set; } = false;
    private readonly ITicketService service;
    readonly ICameraController cameraController;

    public QRScanner(ITicketService service, ICameraController c)
    {
        this.service = service;
        cameraController = c;
    }

    public virtual async Task<bool> DoScanAsync(int event_id)
    {
        SuccessfulScan = false;

        var barcode = await GetScanResultsAsync();
        if (barcode == null) { return false; }

        Ticket t = await service.GetTicket(barcode);

        if (t.EventId != event_id) { return false; }
        if (t.Scanned == true) { return false; }

        t.Scanned = true;
        await service.UpdateTicket(t);

        ScanResult = barcode;
        SuccessfulScan = true;
        return true;
    }

    public virtual async Task<string> GetScanResultsAsync()
    {
        return await cameraController.GetScanResultsAsync();
    }
}