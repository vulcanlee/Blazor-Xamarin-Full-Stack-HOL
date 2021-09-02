using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class CategoryMainAdapterModel : ICloneable
    {
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
        public string EnableName {
            get
            {
                if(Enable)
                {
                    return MagicHelper.EnableTrue;
                }
                else
                {
                    return MagicHelper.EnableFalse;
                }
            }
        }

        public CategoryMainAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as CategoryMainAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
