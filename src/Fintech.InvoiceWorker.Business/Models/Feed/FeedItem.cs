using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoiceWorker.Business.Models.Feed
{
    public class FeedItem
    {
        public int Id { get; set; }

        public EventType Type { get; set; }

        public Invoice Content { get; set; }

        public DateTime CreatedDateUtc { get; set; }
    }
}
