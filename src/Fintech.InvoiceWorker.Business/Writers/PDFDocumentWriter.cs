using Fintech.InvoiceWorker.Business.Models.Feed;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Fintech.InvoiceWorker.Business.Writers
{
    public class PDFDocumentWriter : IInvoiceDocumentWriter
    {
        private const string InvoiceTemplateFileName = "InvoiceTemplate.html";
        private const string RowTemplate = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";

        private readonly ILogger _logger;

        public PDFDocumentWriter(ILogger logger)
        {
            _logger = logger;
        }

        public byte[] CreateDocument(Invoice invoice)
        {
            var template = GetTemplate();
            var lines = new StringBuilder();
            foreach (var line in invoice.Lines)
            {
                var row = string.Format(RowTemplate, line.Description, line.Quantity, line.UnitCost, line.TotalCost);
                lines.Append(row);
            }
            var invoiceHtml = string.Format(template, invoice.Number, lines.ToString());

            return ToPdf(invoiceHtml);
        }

        public static byte[] ToPdf(string html)
        {
            using (var workStream = new MemoryStream())
            {
                using (var pdfWriter = new PdfWriter(workStream))
                {
                    HtmlConverter.ConvertToPdf(html, pdfWriter);
                    return workStream.ToArray();
                }
            }
        }

        private string GetTemplate()
        {
            var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "Templates", InvoiceTemplateFileName);
            
            return File.ReadAllText(path);
        }
    }
}
