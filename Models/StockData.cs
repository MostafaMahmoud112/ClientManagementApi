
using System.ComponentModel.DataAnnotations;
namespace ClientManagement.Models
{
    public class StockData
    {
        public string Ticker { get; set; }
        public int QueryCount { get; set; }
        public int ResultsCount { get; set; }
        public bool Adjusted { get; set; }
        public List<StockQuote> Results { get; set; }
    }

    public class StockQuote
    {
        public int Id { get; set; }
        public decimal? V { get; set; } 
        public decimal? VW { get; set; }
        public decimal? O { get; set; } 
        public decimal? C { get; set; } 
        public decimal? H { get; set; } 
        public decimal? L { get; set; } 
        public long? T { get; set; }
        public int? N { get; set; }
        public DateTime RegDate { get; set; }

    }
}
