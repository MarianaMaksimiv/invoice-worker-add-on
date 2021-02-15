using Fintech.InvoiceWorker.Business.Models;
using Fintech.InvoiceWorker.Business.Repositories;
using Fintech.InvoiceWorker.Business.Services;
using Fintech.InvoiceWorker.Business.Writers;
using Fintech.InvoiceWorker.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fintech.InvoiceWorker.UnitTests.Services
{
    /// <summary>
    /// This was meant to be a component test class but needs more Mocking. Also, I'd prefer to use SpecFlow for this purpose with more time
    /// but for now I'll use it to run the code end-to-end and only mock the Feed.
    /// </summary
    public class CreateInvoicesFilesTests
    {
        private IInvoiceWorkerService _invoiceWorkerService;
        private InvoiceWorkerOptions _options;

        [SetUp]
        public void Setup()
        {
            var logger = new Mock<ILogger>();
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(FeedDataGenerator.GenerateData(3, 0))),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            _options = new InvoiceWorkerOptions
            {
                FeedUrl = "https://localhost:8080",
                InvoicesDirectory = GetTestOutputDirectoryName()
            };

            Console.WriteLine($"Writing PDF Files to '{_options.InvoicesDirectory}'");

            var eventsRepository = new InvoiceEventsRepository(httpClient, _options, logger.Object);
            var pdfWriter = new PDFDocumentWriter(logger.Object);

            // TODO: Mock storage, this won't run in CI/CD like this
            var storage = new FileSystemStorageRepository(_options);
            _invoiceWorkerService = new InvoiceWorkerService(eventsRepository, pdfWriter, storage, _options, logger.Object);
        }

        [Test]
        public async Task CreateInvoicePDFFilesHappyPathTest()
        {
            var initialTotalFiles = Directory.GetFiles(_options.InvoicesDirectory).Length;

            await _invoiceWorkerService.CreateInvoicesDocuments();

            var finalTotalFiles = Directory.GetFiles(_options.InvoicesDirectory).Length;
            Assert.Pass();
            Assert.IsTrue(finalTotalFiles > initialTotalFiles, "Amount of files didn't increase in the target dir");
        }

        private static string GetTestOutputDirectoryName()
        {
            var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "OutputFiles");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
