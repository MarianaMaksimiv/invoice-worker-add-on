using Fintech.InvoiceWorker.Business.Models;
using Fintech.InvoiceWorker.Business.Models.Feed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Services
{
    public class InvoiceWorkerService : IInvoiceWorkerService
    {
        private readonly InvoiceWorkerOptions _options;

        public InvoiceWorkerService(InvoiceWorkerOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Reads data from a feed with invoices and converts the json detail to PDF
        /// </summary>
        public async Task CreateInvoicesDocuments()
        {
            ValidateInputParameters(_options.FeedUrl, _options.InvoicesDirectory);
        }

        private List<Invoice> ReadInvoices()
        {
            return null;
        }

        private void ValidateInputParameters(string feedUrl, string invoicesDirectory)
        {
            if (string.IsNullOrEmpty(feedUrl))
            {
                throw new ArgumentNullException("Feed url parameter must be provided.");
            }

            if (string.IsNullOrEmpty(invoicesDirectory))
            {
                throw new ArgumentNullException("The directory to store invoices must be provided.");
            }

            bool isValidFeedUrl = Uri.TryCreate(feedUrl, UriKind.Absolute, out var uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!isValidFeedUrl)
            {
                throw new ArgumentNullException("Feed url parameter must be a valid url (http/https).");
            }

            if (!Directory.Exists(invoicesDirectory))
            {
                throw new ArgumentNullException($"The directory to store invoices doesn't exist: {invoicesDirectory}.");
            }
        }
    }
}
