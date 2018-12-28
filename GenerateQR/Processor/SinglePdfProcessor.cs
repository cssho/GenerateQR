using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateQR.Properties;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GenerateQR.Processor
{
    public class SinglePdfProcessor
    {
        private readonly PrintData print;
        private static readonly float QrCodeSize = 80f;
        private static readonly float LogoSize = 40f;

        public SinglePdfProcessor(PrintData print)
        {
            this.print = print;
        }

        public void Print()
        {
            using (var reader = new PdfReader(Resources.SnsTemplate))
            {
                var fs = new FileStream(print.OutputPath, FileMode.Create, FileAccess.Write);
                var doc = new Document(PageSize.A5);
                var pw = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                var pdfContentByte = pw.DirectContent;
                var page = pw.GetImportedPage(reader, 1);
                pdfContentByte.AddTemplate(page, 0, 0);

                var bf = BaseFont.CreateFont(@"c:\windows\fonts\BIZ-UDGothicR.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                pdfContentByte.SetFontAndSize(bf, 30);
                pdfContentByte.BeginText();
                pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, print.DisplayName,
                    page.Width / 2, page.Height - 90, 0);
                pdfContentByte.EndText();

                var table = new PdfPTable(3);
                var tableBodyWidth = (float)(page.Width * 0.86 - QrCodeSize * 2);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                table.SetTotalWidth(new[] { QrCodeSize, tableBodyWidth, QrCodeSize });
                table.LockedWidth = true;
                table.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                var cellFont = new Font(bf, 24);
                var alignLeft = true;
                foreach (var sns in print.Sns.Where(x => x != null))
                {
                    if (alignLeft)
                        table.AddCell(Image.GetInstance(sns.QrCodeImage, ImageFormat.Bmp));
                    var nest = new PdfPTable(2);
                    nest.DefaultCell.Border = Rectangle.NO_BORDER;
                    nest.SetTotalWidth(alignLeft
                        ? new[] { LogoSize, tableBodyWidth - LogoSize }
                        : new[] { tableBodyWidth - LogoSize, LogoSize });
                    nest.LockedWidth = true;
                    nest.HorizontalAlignment = alignLeft
                        ? PdfContentByte.ALIGN_LEFT
                        : PdfContentByte.ALIGN_RIGHT;
                    InsertLogoAndText(cellFont, nest,
                        Image.GetInstance(sns.LoadImage(), BaseColor.WHITE),
                        sns.SnsName, alignLeft);

                    InsertLogoAndText(cellFont, nest,
                        Image.GetInstance(sns.ProfileImage, BaseColor.WHITE),
                        sns.DisplayAccount, alignLeft);
                    var bodyCell = new PdfPCell(nest)
                    {
                        Colspan = 2,
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = alignLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT
                    };
                    table.AddCell(bodyCell);
                    if (!alignLeft)
                        table.AddCell(Image.GetInstance(sns.QrCodeImage, ImageFormat.Bmp));

                    InsertLine(table, alignLeft);
                    alignLeft = !alignLeft;
                }

                table.WriteSelectedRows(0, -1, (float)(page.Width * 0.07), page.Height - 160, pdfContentByte);
                doc.Close();
                fs.Close();
                pw.Close();
            }

        }

        private static void InsertLine(PdfPTable table, bool alignLeft)
        {
            if (!alignLeft) table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });
            var lineCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Colspan = 2,
                BackgroundColor = BaseColor.GRAY,
                FixedHeight = 3
            };
            table.AddCell(lineCell);

            if (alignLeft) table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Colspan = 3,
                FixedHeight = 8
            });
        }

        private static void InsertLogoAndText(Font cellFont, PdfPTable nest,
            Image image, string text, bool alignLeft)
        {
            if (alignLeft)
            {
                nest.AddCell(image);
                var snsCell = new PdfPCell(new Phrase(text, cellFont))
                {
                    Border = Rectangle.NO_BORDER
                };
                nest.AddCell(snsCell);
            }
            else
            {

                var snsCell = new PdfPCell(new Phrase(text, cellFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT

                };
                nest.AddCell(snsCell);
                nest.AddCell(image);
            }
        }
    }
}
