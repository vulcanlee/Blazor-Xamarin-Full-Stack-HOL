using System;
using System.Collections.Generic;

#nullable disable

namespace Domains.Models
{
    public partial class OrderMaster
    {
        public OrderMaster()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
