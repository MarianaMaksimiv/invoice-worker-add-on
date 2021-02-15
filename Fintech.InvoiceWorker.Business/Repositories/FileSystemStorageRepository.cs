using Fintech.InvoiceWorker.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoicesWorker.Business.Repositories
{
    /// <summary>
    /// Repository to store files in a directory from local file system.
    /// </summary>
    public class FileSystemStorageRepository : IInvoiceStorageRepository
    {
        private InvoiceWorkerOptions _options;

        public FileSystemStorageRepository(InvoiceWorkerOptions options)
        {
            _options = options;
        }

        public double SpaceLeftInMB()
        {
            throw new NotImplementedException();
        }

        public void WriteFile(string fileName, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
