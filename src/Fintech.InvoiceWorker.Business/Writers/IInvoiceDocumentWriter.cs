using Fintech.InvoiceWorker.Business.Models.Feed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoiceWorker.Business.Writers
{
    public interface IInvoiceDocumentWriter
    {
        byte[] CreateDocument(Invoice invoice);
    }
}
