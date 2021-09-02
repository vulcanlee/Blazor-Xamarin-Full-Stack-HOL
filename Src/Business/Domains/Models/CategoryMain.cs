using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class CategoryMain
    {
        public virtual ICollection<CategorySub> CategorySub { get; set; }
        public CategoryMain()
        {
            CategorySub = new HashSet<CategorySub>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }
    }
}