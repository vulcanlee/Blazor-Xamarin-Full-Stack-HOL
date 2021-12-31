using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class InitDatas
    {
        public List<AccountPolicy> AccountPolicy { get; set; } = new();
        public List<MenuRole> MenuRole { get; set; } = new();
        public List<MenuData> MenuData { get; set; } = new();
        public List<MyUser> MyUser { get; set; } = new();
        public List<MyUserPasswordHistory> MyUserPasswordHistory { get; set; } = new();
    }
}