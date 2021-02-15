using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoiceWorker.Business.Models
{
    public class InvoiceWorkerOptions
    {
        public string FeedUrl { get; set; }

        public string InvoicesDirectory { get; set; }

        public string MinimumStorageMBRequired { get; set; }
    }
}
