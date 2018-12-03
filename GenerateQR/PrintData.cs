using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateQR
{
    public class PrintData
    {
        public string DisplayName { get; set; }
        public SnsData[] Sns { get; set; }
        public string OutputPath { get; set; }
    }
}
