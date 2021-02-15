using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Repositories
{
    public interface IInvoiceStorageRepository
    {
        /// <summary>
        /// Writes files using the file name provided.
        /// If there is an existing file it will overwrite the content.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        Task WriteFile(string fileName, byte[] data);

        double SpaceLeftInMB();
    }
}
