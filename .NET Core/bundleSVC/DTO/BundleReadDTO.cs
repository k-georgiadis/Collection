using System;
using System.ComponentModel.DataAnnotations;

namespace BundleSVC.DTO
{
    public class BundleReadDTO
    {
        public int B_code { get; set; }
        public string B_name { get; set; }
        public double B_price { get; set; }
        public DateTime B_expdate { get; set; }
        public DateTime B_availdate { get; set; }
        public bool B_active { get; set; }
    }
}
