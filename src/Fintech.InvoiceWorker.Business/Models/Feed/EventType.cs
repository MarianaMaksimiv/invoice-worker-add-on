using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoiceWorker.Business.Models.Feed
{
    public enum EventType
    {
        INVOICE_CREATED,
        INVOICE_UPDATED,
        INVOICE_DELETED
    }
}
