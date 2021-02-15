using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoicesWorker.Business.Repositories
{
    public interface IInvoiceStorageRepository
    {
        /// <summary>
        /// Writes files using the file name provided.
        /// If there is an existing file it will overwrite the content.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public void WriteFile(string fileName, byte[] data);

        public double SpaceLeftInMB();
    }
}
