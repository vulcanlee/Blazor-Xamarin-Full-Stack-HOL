using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public partial class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public short ModelYear { get; set; }
        public decimal ListPrice { get; set; }
    }
}
