using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UPC { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}