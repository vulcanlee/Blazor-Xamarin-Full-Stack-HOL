using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public partial class OrderDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public virtual ICollection<OrderItemDto> OrderItem { get; set; }
    }
}
