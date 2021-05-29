﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.AdapterModels
{
    public class MenuDataAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        [Range(0, 1, ErrorMessage = "僅能輸入 0 或者 1 不可為空白")]
        public int Level { get; set; }
        public bool IsGroup { get; set; }
        public string IsGroupName { get; set; }
        public int Sequence { get; set; }
        public string Icon { get; set; }
        [Required(ErrorMessage = "作業名稱 不可為空白")]
        public string CodeName { get; set; }
        public bool Enable { get; set; }
        public string EnableName { get; set; }
        [Required(ErrorMessage = "驗證碼 不可為空白")]
        public string VerifyCode { get; set; }
        public Guid Guid { get; set; }
        public bool ForceLoad { get; set; }
        public int MenuRoleId { get; set; }
        public virtual MenuRoleAdapterModel MenuRole { get; set; }

        public MenuDataAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MenuDataAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
