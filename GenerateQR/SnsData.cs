using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateQR
{
    public abstract class SnsData
    {
        protected abstract Uri SnsBaseUri { get; }
        public abstract string SnsName { get; }
        public string DisplayName { get; set; }
        public string Account { get; set; }
        public Bitmap QrCodeImage { get; set; }
        public Bitmap ProfileImage { get; set; }

        public virtual Uri ProfileUri => new Uri(SnsBaseUri, Account);

        public virtual string DisplayAccount => $"@{Account}";
    }

    public class TwitterData : SnsData
    {
        protected override Uri SnsBaseUri => new Uri("https://twitter.com");
        public override string SnsName => "Twitter";

    }

    public class FacebookData : SnsData
    {
        protected override Uri SnsBaseUri => new Uri("https://www.facebook.com");
        public override string SnsName => "Facebook";

    }

    public class InstagramData : SnsData
    {
        protected override Uri SnsBaseUri => new Uri("https://www.instagram.com");
        public override string SnsName => "Instagram";

    }

    public class AmebloData : SnsData
    {
        protected override Uri SnsBaseUri => new Uri("https://ameblo.jp");
        public override string SnsName => "アメブロ";

        public override string DisplayAccount => Account;

    }
}
