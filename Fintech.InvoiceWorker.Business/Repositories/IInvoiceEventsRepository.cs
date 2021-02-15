using Fintech.InvoiceWorker.Business.Models.Feed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Repositories
{
    public interface IInvoiceEventsRepository
    {
        Task<List<Invoice>> GetInvoicesEvents(int pageSize, int afterEventId);
    }
}
