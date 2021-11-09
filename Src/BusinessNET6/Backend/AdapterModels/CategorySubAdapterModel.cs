using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class CategorySubAdapterModel : ICloneable
    {
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public bool Enable { get; set; }
        public string EnableName
        {
            get
            {
                if (Enable)
                {
                    return MagicHelper.EnableTrue;
                }
                else
                {
                    return MagicHelper.EnableFalse;
                }
            }
        }
        public int CategoryMainId { get; set; }
        public CategoryMainAdapterModel CategoryMain { get; set; }
        public string CategoryMainName { get; set; }

        public CategorySubAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as CategorySubAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
