using Fintech.InvoiceWorker.Business.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Repositories
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

        public async Task WriteFile(string fileName, byte[] data)
        {
            var path = Path.Combine(_options.InvoicesDirectory, fileName);
            // This overwrites the file if it already exists
            await File.WriteAllBytesAsync(path, data);
        }
    }
}
