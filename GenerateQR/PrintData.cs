using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateQR.Processor;

namespace GenerateQR
{
    public class PrintData
    {
        public async Task Load(InputData input)
        {
            DisplayName = input.DisplayName;
            Sns = await Task.WhenAll(
                SnsProcessorFactory.Create<TwitterData>().LoadData(input.TwitterAccount),
                SnsProcessorFactory.Create<FacebookData>().LoadData(input.FacebookAccount),
                SnsProcessorFactory.Create<InstagramData>().LoadData(input.InstagramAccount),
                SnsProcessorFactory.Create<AmebloData>().LoadData(input.AmebloAccount));
            OutputPath = $"out_{DisplayName}_{DateTime.Now.ToString("yyyyMMddhhmm")}.pdf";
        }

        public string DisplayName { get; private set; }
        public SnsData[] Sns { get; private set; }
        public string OutputPath { get; private set; }
    }
}
