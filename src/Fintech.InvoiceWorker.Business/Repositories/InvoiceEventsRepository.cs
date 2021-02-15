using Fintech.InvoiceWorker.Business.Models;
using Fintech.InvoiceWorker.Business.Models.Feed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.Business.Repositories
{
    /// <summary>
    /// Reads invoices data from a Feed
    /// </summary>
    public class InvoiceEventsRepository : IInvoiceEventsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _feedUrl;
        private readonly ILogger _logger;

        public InvoiceEventsRepository(HttpClient client, InvoiceWorkerOptions options, ILogger logger)
        {
            _httpClient = client;
            _feedUrl = options.FeedUrl;
            _logger = logger;
        }

        /// <summary>
        /// Reads data from the feed using default parameters and returns a list of invoices
        /// </summary>
        /// <param name="pageSize">Default value 10</param>
        /// <param name="afterEventId">Default value 0, means the latest more recent event</param>
        /// <returns></returns>
        public async Task<List<FeedItem>> GetInvoicesEvents(int pageSize = 10, int afterEventId = 0)
        {
            _logger.LogDebug($"Sending GET request to the Invoices Feed {_feedUrl}");

            var response = await _httpClient.GetAsync(_feedUrl);

            _logger.LogDebug($"Got response from Feed with a {response.StatusCode} status code");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var feedContent = JsonConvert.DeserializeObject<FeedContent>(responseString);

            return feedContent.Items;
        }
    }
}
