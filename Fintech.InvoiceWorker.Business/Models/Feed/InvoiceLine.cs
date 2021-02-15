using Newtonsoft.Json;

namespace Fintech.InvoiceWorker.Business.Models.Feed
{
    public class InvoiceLine
    {
        [JsonProperty("lineItemId")]
        public string Id { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double UnitCost { get; set; }

        [JsonProperty("lineItemTotalCost")]
        public double TotalCost { get; set; }
    }
}
