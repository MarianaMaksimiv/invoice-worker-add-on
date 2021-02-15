using Fintech.InvoiceWorker.Business.Models;
using Fintech.InvoiceWorker.Business.Models.Feed;
using Fintech.InvoiceWorker.Business.Repositories;
using Fintech.InvoiceWorker.Business.Writers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Services
{
    public class InvoiceWorkerService : IInvoiceWorkerService
    {
        private readonly IInvoiceEventsRepository _invoiceEventsRepository;
        private readonly IInvoiceDocumentWriter _documentWriter;
        private readonly IInvoiceStorageRepository _storageRepository;
        private readonly InvoiceWorkerOptions _options;
        private readonly ILogger _logger;

        public InvoiceWorkerService(IInvoiceEventsRepository invoiceEventsRepository, IInvoiceDocumentWriter documentWriter, IInvoiceStorageRepository storageRepository, InvoiceWorkerOptions options, ILogger logger)
        {
            _options = options;
            _logger = logger;
            _invoiceEventsRepository = invoiceEventsRepository;
            _documentWriter = documentWriter;
            _storageRepository = storageRepository;
        }

        // TODO: Add storage to keep this value if the User wants to start from the last event read/ invoice created
        public int LastEventRead { get; set; } = 0;

        /// <summary>
        /// Reads data from a feed with invoices and converts the json detail to PDF
        /// </summary>
        public async Task CreateInvoicesDocuments()
        {
            _logger.LogInformation("Atempting to process invoices...");
            ValidateInputParameters(_options.FeedUrl, _options.InvoicesDirectory);

            try
            {
                // TODO: move polling inside of the service
                var events = await ReadInvoices();

                foreach (var invoiceEvent in events)
                {
                    switch (invoiceEvent.Type)
                    {
                        case EventType.INVOICE_CREATED:
                            _logger.LogInformation("Atempting to generate the PDF document");
                            var pdfContent = _documentWriter.CreateDocument(invoiceEvent.Content);

                            _logger.LogInformation("Saving PDF to a file");
                            await _storageRepository.WriteFile(invoiceEvent.Content.Id, pdfContent);
                            break;
                        default:
                            // TODO: Handle Deletes and Updates
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while processing invoices", e);
                throw;
            }
        }

        private async Task<List<FeedItem>> ReadInvoices()
        {
            var pageSize = 10;
            var allInvoicesEvents = await _invoiceEventsRepository.GetInvoicesEvents(pageSize, LastEventRead);

            // Group the events by invoice Id and pick up the latest one of each group
            var filteredEvents = allInvoicesEvents?.GroupBy(i => i.Id, (key, g) => g.OrderByDescending(e => e.Content.UpdatedDateUtc).First()).ToList();
            LastEventRead = allInvoicesEvents.Last().Id;

            return filteredEvents;
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
