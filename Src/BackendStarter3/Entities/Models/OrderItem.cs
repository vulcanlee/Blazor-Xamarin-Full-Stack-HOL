using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.Models
{
    public partial class OrderItem
    {
        public int Id { get; set; }
        public int OrderMasterId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }
        public virtual Product Product { get; set; }
    }
}
