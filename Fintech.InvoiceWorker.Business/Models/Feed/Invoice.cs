using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fintech.InvoiceWorker.Business.Models.Feed
{
    /// <summary>
    /// Feed item with invoice details
    /// </summary>
    public class Invoice
    {
        [JsonProperty("invoiceId")]
        public string Id { get; set; }


        [JsonProperty("invoiceNumber")]
        public string Number { get; set; }


        [JsonProperty("lineItems")]
        public List<InvoiceLine> Lines { get; set; }

        public InvoiceStatus Status { get; set; }

        public DateTime DueDateUtc { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime UpdatedDateUtc { get; set; }
    }
}
