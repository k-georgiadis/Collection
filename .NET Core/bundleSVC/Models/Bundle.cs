using System;
using System.ComponentModel.DataAnnotations;

namespace bundleSVC.Models
{
    public class Bundle
    {
        [Key]
        public int B_code { get; set; }

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
