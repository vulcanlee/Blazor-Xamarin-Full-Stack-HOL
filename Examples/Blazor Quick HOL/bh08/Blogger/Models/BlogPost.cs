using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="文章主題 必須存在")]
        public string Title { get; set; }
        [Required(ErrorMessage ="文章內容 必須存在")]
        public string Text { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
    }
}
