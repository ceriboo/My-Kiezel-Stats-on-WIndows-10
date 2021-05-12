using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datakiezel
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Purchases
    {
        public int total { get; set; }
        public string periodStart { get; set; }
        public string periodEnd { get; set; }
    }

    public class NextPayout
    {
        public string date { get; set; }
        public double amount { get; set; }
        public Purchases purchases { get; set; }
    }

    public class PreviousPayout
    {
        public string date { get; set; }
        public double amount { get; set; }
        public Purchases purchases { get; set; }
    }

    public class datasummary
    {
        public int totalPurchases { get; set; }
        public int totalActiveTrials { get; set; }
        public double totalIncome { get; set; }
        public double totalPaidOut { get; set; }
        public double currentBalance { get; set; }
        public NextPayout nextPayout { get; set; }
        public PreviousPayout previousPayout { get; set; }
    }


}
