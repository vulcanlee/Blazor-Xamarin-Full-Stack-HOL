using System;
using System.Collections.Generic;

#nullable disable

namespace Entities.Models
{
    public partial class Holuser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int TokenVersion { get; set; }
        public int Level { get; set; }
    }
}
