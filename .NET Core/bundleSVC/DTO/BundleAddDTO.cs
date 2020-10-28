using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bundleSVC.DTO
{
    public class BundleAddDTO
    {
        [Required]
        [MaxLength(100)]
        public string B_name { get; set; }

        [Required]
        public double B_price { get; set; }

        public DateTime B_expdate { get; set; }

        [Required]
        public DateTime B_availdate { get; set; }

        [Required]
        public bool B_active { get; set; }
    }
}
