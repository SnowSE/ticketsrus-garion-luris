using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

using QRCoder;
namespace TicketsRUs.ClassLib;

#pragma warning disable CA1416

public static class QRCodeToolkit
{
    public static Bitmap GenerateQRCodeFromString(string s)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(s, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public static byte[] ConvertToByteArray(Bitmap qr)
    {
        byte[] result;
        using (MemoryStream stream = new MemoryStream())
        {
            qr.Save(stream, ImageFormat.Png);
            result = stream.ToArray();
        }

        return result;
    }
}

#pragma warning restore CA1416