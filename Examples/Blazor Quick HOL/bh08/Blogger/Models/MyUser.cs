using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Models
{
    public class MyUser
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
