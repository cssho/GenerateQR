using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace GenerateQR.Processor
{
    public static class QrCreator
    {
        public static Bitmap Create(string uri, Bitmap logo)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            {
                return qrCode.GetGraphic(20, Color.Black, Color.White, logo);
            }
        }
    }
}
