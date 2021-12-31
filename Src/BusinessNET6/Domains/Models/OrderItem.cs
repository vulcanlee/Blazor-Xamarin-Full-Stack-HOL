using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderMasterId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [Precision(precision: 10, scale: 2)]
        public decimal ListPrice { get; set; }
        [Precision(precision: 5, scale: 2)]
        public decimal Discount { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }
        public virtual Product Product { get; set; }
    }
}
