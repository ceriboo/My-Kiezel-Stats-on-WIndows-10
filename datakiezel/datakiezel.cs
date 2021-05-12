using System;

namespace datakiezel
{
    
   
    public class Datakiezel
    {
        public string date { get; set; }
        public int purchases { get; set; }
        public double amount { get; set; }
        public int rank { get; set; }
    }

    public class mydata
    {
        public Datakiezel datakiezel { get; set; }
    }
}
