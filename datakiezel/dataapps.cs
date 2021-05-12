using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datakiezel
{
    public class Product
    {
        public string name { get; set; }
        public int id { get; set; }
        public string price { get; set; }
        public string description { get; set; }
        public string pebbleAppstoreUrl { get; set; }
        public string fitbitAppstoreUrl { get; set; }
        public string garminAppstoreUrl { get; set; }
        public string image { get; set; }
        public string shop { get; set; }
        public bool isBundle { get; set; }
        public List<int> bundles { get; set; }
    }

    public class Bundle
    {
        public string name { get; set; }
        public int id { get; set; }
        public string price { get; set; }
        public string description { get; set; }
        public List<int> apps { get; set; }
        string Listofapps { get; set; }
        public string overview { get; set; }
        public string shop { get; set; }
        public bool isBundle { get; set; }
        public bool active { get; set; }
    }

    public class dataapps
    {
        public List<Product> products { get; set; }
        public List<Bundle> bundles { get; set; }
    }
}
