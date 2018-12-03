using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateQR.Properties;

namespace GenerateQR.Processor
{
    public static class SnsProcessorFactory
    {
        public static BaseSnsProcessor<T> Create<T>() where T : SnsData, new()
        {
            if (typeof(T) == typeof(TwitterData))
                return new TwitterProcessor() as BaseSnsProcessor<T>;
            if (typeof(T) == typeof(FacebookData))
                return new FacebookProcessor() as BaseSnsProcessor<T>;
            if (typeof(T) == typeof(InstagramData))
                return new InstagramProcessor() as BaseSnsProcessor<T>;
            if (typeof(T) == typeof(AmebloData))
                return new AmebloProcessor() as BaseSnsProcessor<T>;
            throw new NotImplementedException();
        }

        public static Bitmap LoadImage(this SnsData sns)
        {
            if (sns.GetType() == typeof(TwitterData))
                return Resources.TwitterLogo;
            if (sns.GetType() == typeof(FacebookData))
                return Resources.FacebookLogo;
            if (sns.GetType() == typeof(InstagramData))
                return Resources.InstagramLogo;
            if (sns.GetType() == typeof(AmebloData))
                return Resources.AmebaLogo;
            throw new NotImplementedException();
        }
    }
}
