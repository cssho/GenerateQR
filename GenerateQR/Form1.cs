using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerateQR.Processor;

namespace GenerateQR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbDisplayName.Text))
            {
                MessageBox.Show(this, "表示名を入力してください");
                return;
            }
            var pr = new PrintData()
            {
                DisplayName = tbDisplayName.Text,
                Sns = await Task.WhenAll(
                    SnsProcessorFactory.Create<TwitterData>().LoadData(tbTwitter.Text),
                    SnsProcessorFactory.Create<FacebookData>().LoadData(tbFacebook.Text),
                    SnsProcessorFactory.Create<InstagramData>().LoadData(tbInstagram.Text),
                    SnsProcessorFactory.Create<AmebloData>().LoadData(tbAmeblo.Text)),
                OutputPath = $"out_{DateTime.Now.ToString("yyyyMMddhhmm")}.pdf"
            };
            var printer = new PdfProcessor(pr);
            try
            {
                printer.Print();
                MessageBox.Show(this, "出力が完了しました。");
                Process.Start(pr.OutputPath);
            }
            catch (Exception xe)
            {
                MessageBox.Show(this, xe.ToString());
            }
        }
    }
}
