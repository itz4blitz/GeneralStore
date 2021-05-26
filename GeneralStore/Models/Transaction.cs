using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.Models
{
    public class Transaction
    {
        //Links two tables together through itself
        //Joining/Junction table

        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }
        [Required]
        public int PurchaseQuantity { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateOfTransaction { get; set; }

    }
}