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
using Newtonsoft.Json;

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

            var input = new InputData()
            {
                DisplayName = tbDisplayName.Text,
                TwitterAccount = tbTwitter.Text,
                FacebookAccount = tbFacebook.Text,
                InstagramAccount = tbInstagram.Text,
                AmebloAccount = tbAmeblo.Text
            };
            File.WriteAllText($"{input.DisplayName}.json", JsonConvert.SerializeObject(input));
            var pr = new PrintData();
            await pr.Load(input);
            var printer = new SinglePdfProcessor(pr);
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
