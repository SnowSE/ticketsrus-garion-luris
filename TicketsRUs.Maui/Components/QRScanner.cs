﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace TicketsRUs.Maui.Components;

public class QRScanner
{
    public string ScanResult { get; private set; } = "";
    public bool SuccessfulScan { get; private set; } = false;

    public async Task DoScanAsync()
    {
        SuccessfulScan = false;

        var barcode = await GetScanResultsAsync();
        if (barcode != null)
        {
            ScanResult = barcode.Value;
            SuccessfulScan = true;
        }
    }

    public async Task<BarcodeResult> GetScanResultsAsync()
    {
        var cameraPage = new CameraPage();

        await Application.Current.MainPage.Navigation.PushModalAsync(cameraPage);
        var results = await cameraPage.WaitForResultAsync();
        await Application.Current.MainPage.Navigation.PopModalAsync();

        return results;
    }
}