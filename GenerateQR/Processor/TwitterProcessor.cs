using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using GenerateQR.Properties;

namespace GenerateQR.Processor
{
    public class TwitterProcessor : BaseSnsProcessor<TwitterData>
    {
        public override async Task<SnsData> LoadData(string account)
        {
            try
            {
                var data = new TwitterData { Account = account };
                var parser = new HtmlParser();
                var html = await Client.GetStringAsync(data.ProfileUri.ToString());
                var document = await parser.ParseAsync(html);
                var img = document.QuerySelector(".ProfileAvatar-image");
                data.DisplayName = img.Attributes["alt"].Value;
                data.ProfileImage = await GetBitmap(img.Attributes["src"].Value);

                data.QrCodeImage = QrCreator.Create(data.ProfileUri.ToString(),
                    Resources.TwitterLogo);
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
