using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Services
{
    public interface IInvoiceWorkerService
    {
        Task CreateInvoicesDocuments();
    }
}
