using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using GenerateQR.Properties;

namespace GenerateQR.Processor
{
    public class AmebloProcessor : BaseSnsProcessor<AmebloData>
    {
        public override async Task<SnsData> LoadData(string account)
        {
            try
            {
                var data = new AmebloData() { Account = account };

                var document = await BrowsingContext
                    .New(Configuration.Default.WithDefaultLoader())
                    .OpenAsync(data.ProfileUri.ToString());
                data.DisplayName = document.QuerySelector("title").TextContent;
                data.ProfileImage = await GetBitmap(
                    document.QuerySelector("#profile > div > div.skin-widgetBody > div.skin-profile > div > a > img")
                        .Attributes["src"].Value);

                data.QrCodeImage = QrCreator.Create(data.ProfileUri.ToString(),
                    Resources.AmebaLogo);
                return data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}