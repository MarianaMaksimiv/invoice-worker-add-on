using Fintech.InvoiceWorker.Business.Models.Feed;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fintech.InvoiceWorker.UnitTests.Helpers
{
    public static class FeedDataGenerator
    {
        private readonly static Random _rd = new Random();

        // This might be a problem with parallel tests, but keeping it simple for now.
        private static int _lastEventId;

        public static FeedContent GenerateData(int pageSize, int afterEventId)
        {
            // TODO: Also include UPDATES and DELETES
            var feed = new FeedContent
            {
                Items = GenerateCreateEvents(pageSize)
            };

            return feed;
        }

        private static List<FeedItem> GenerateCreateEvents(int totalEvents)
        {
            var items = new List<FeedItem>();
            for (var i = 0; i < totalEvents; i++)
            {
                _lastEventId++;
                // TODO: Improve data quality to be more random
                var lines = GenerateLines();
                var now = DateTime.UtcNow;
                var invoice = new Invoice
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = InvoiceStatus.DRAFT,
                    Number = $"INV-00{i}",
                    Lines = lines,
                    CreatedDateUtc = now,
                    UpdatedDateUtc = now,
                    DueDateUtc = now
                };

                items.Add(new FeedItem
                {
                    Id = _lastEventId,
                    Type = EventType.INVOICE_CREATED,
                    Content = invoice,
                    CreatedDateUtc = now
                });
            }

            return items;
        }

        private static List<InvoiceLine> GenerateLines()
        {
            var lines = new List<InvoiceLine>();

            // TODO: For now let's only generate this amount of lines, we should check real data and find how big the invoices can be
            var totalLines = _rd.Next(1, 21);
            for (var i = 0; i < totalLines; i++)
            {
                var quantity = _rd.Next(1, 100);
                var unitCost = _rd.Next(50, 500) * _rd.NextDouble();

                var line = new InvoiceLine
                {
                    Description = $"Supplies {i + 1}",
                    Id = Guid.NewGuid().ToString(),
                    Quantity = quantity,
                    UnitCost = unitCost,
                    TotalCost = quantity * unitCost
                };

                lines.Add(line);
            }

            return lines;
        }
    }
}
