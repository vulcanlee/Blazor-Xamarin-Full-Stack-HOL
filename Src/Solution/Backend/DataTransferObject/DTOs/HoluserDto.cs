using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public partial class HoluserDto
    {
        public int HoluserId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int TokenVersion { get; set; }
        public int Level { get; set; }
    }
}
