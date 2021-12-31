using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public short ModelYear { get; set; }
        [Precision(precision: 10, scale: 2)]
        public decimal ListPrice { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
