using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemClassLibrary.Entity {
    public class ItemHistory {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Classification { get; set; }
        public Dictionary<DateTime, decimal> HistoryCount { get; set; }
        public Dictionary<DateTime, decimal> HistoryPrice { get; set; }
        public Dictionary<DateTime, decimal> HistoryPriceStar1 { get; set; }
        public Dictionary<DateTime, decimal> HistoryPriceStar2 { get; set; }
        public Dictionary<DateTime, decimal> HistoryPriceStar3 { get; set; }
        public Dictionary<DateTime, decimal> NowPrice { get; set; }
        public Dictionary<DateTime, decimal> NowPriceStar1 { get; set; }
        public Dictionary<DateTime, decimal> NowPriceStar2 { get; set; }
        public Dictionary<DateTime, decimal> NowPriceStar3 { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }

        public ItemHistory() {
            this.HistoryCount = new Dictionary<DateTime, decimal>();
            this.HistoryPrice = new Dictionary<DateTime, decimal>();
            this.HistoryPriceStar1 = new Dictionary<DateTime, decimal>();
            this.HistoryPriceStar2 = new Dictionary<DateTime, decimal>();
            this.HistoryPriceStar3 = new Dictionary<DateTime, decimal>();
            this.NowPrice = new Dictionary<DateTime, decimal>();
            this.NowPriceStar1 = new Dictionary<DateTime, decimal>();
            this.NowPriceStar2 = new Dictionary<DateTime, decimal>();
            this.NowPriceStar3= new Dictionary<DateTime, decimal>();
        }
    }
}
