using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.Maui.Controllers;
using ZXing.Net.Maui;

namespace TicketsRUs.Maui.Components;

public class QRScanner
{
    public string ScanResult { get; private set; } = "";
    public bool SuccessfulScan { get; private set; } = false;
    private ITicketService service;
    ICameraController cameraController;

    public QRScanner(ITicketService service)
    {
        this.service = service;
        cameraController = new CameraController();
    }

    public QRScanner(ITicketService service, ICameraController c)
    {
        this.service = service;
        cameraController = c;
    }

    public virtual async Task DoScanAsync()
    {
        SuccessfulScan = false;

        var barcode = await GetScanResultsAsync();
        if (barcode != null)
        {
            Ticket t = await service.GetTicket(barcode);
            t.Scanned = true;
            await service.UpdateTicket(t);
            
            ScanResult = barcode;
            SuccessfulScan = true;
        }
    }

    public virtual async Task<string> GetScanResultsAsync()
    {
        return await cameraController.GetScanResultsAsync();
    }
}

public class CameraController() : ICameraController
{
    public async Task<string> GetScanResultsAsync()
    {
        var cameraPage = new CameraPage();

        await Application.Current.MainPage.Navigation.PushModalAsync(cameraPage);
        var results = await cameraPage.WaitForResultAsync();
        await Application.Current.MainPage.Navigation.PopModalAsync();

        return results.Value;
    }
}

public interface ICameraController
{
    Task<string> GetScanResultsAsync();
}
