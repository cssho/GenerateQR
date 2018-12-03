using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using GenerateQR.Properties;

namespace GenerateQR.Processor
{

    public abstract class BaseSnsProcessor<T> where T : SnsData, new()
    {
        protected static readonly HttpClient Client = new HttpClient();

        public virtual async Task<SnsData> LoadData(string account)
        {
            if (string.IsNullOrEmpty(account)) return null;
            try
            {
                var data = new T { Account = account };

                var document = await BrowsingContext
                    .New(Configuration.Default.WithDefaultLoader())
                    .OpenAsync(data.ProfileUri.ToString());
                var meta = document.QuerySelectorAll("head > meta");
                data.DisplayName = meta.First(x => x.HasAttribute("property")
                                                   && x.Attributes["property"].Value == "og:title")
                    .Attributes["content"].Value;
                data.ProfileImage = await GetBitmap(meta
                    .First(x => x.HasAttribute("property")
                                && x.Attributes["property"].Value == "og:image").Attributes["content"].Value);

                data.QrCodeImage = QrCreator.Create(data.ProfileUri.ToString(),
                    data.LoadImage());
                return data;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }


        protected virtual async Task<Bitmap> GetBitmap(string uri)
        {
            using (var ms = await Client.GetStreamAsync(uri))
            {
                return new Bitmap(ms);
            }
        }
    }


}