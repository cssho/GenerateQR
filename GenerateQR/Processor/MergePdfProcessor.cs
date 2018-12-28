using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GenerateQR.Processor
{
    public class MergePdfProcessor
    {
        private readonly string[] paths;

        public MergePdfProcessor(string[] paths)
        {
            this.paths = paths;
            OutputPath = $"merge_{DateTime.Now.ToString("yyyyMMddhhmm")}.pdf";
        }

        public readonly string OutputPath;


        public void Merge()
        {
            // PDFドキュメント作成
            var joinDc = new Document(PageSize.A4);
            // 結合先PDFファイルの作成
            var joinFs = new FileStream(OutputPath, FileMode.Create, FileAccess.Write);
            // 結合先PDFオブジェクトとPDFファイルの関連付け
            var joinWr = PdfWriter.GetInstance(joinDc, joinFs);
            // 結合先PDFオープン
            joinDc.Open();
            // PdfContentByte取得
            var joinPcb = joinWr.DirectContent;
            var newPage = true;
            foreach (var fileName in paths)
            {
                Console.WriteLine($"merging:{fileName}");
                // ファイル読み込み
                var joinRd = new PdfReader(File.ReadAllBytes(fileName));

                // 改ページ
                if (newPage)
                    joinDc.NewPage();
                // ページ取得
                var joinPage = joinWr.GetImportedPage(joinRd, 1);
                joinPcb.AddTemplate(joinPage, 0, -1, 1, 0, 0, PageSize.A5.Width * (newPage ? 2 : 1));
                newPage = !newPage;

            }
            // 結合先PDFクローズ
            joinDc.Close();
        }
    }
}
