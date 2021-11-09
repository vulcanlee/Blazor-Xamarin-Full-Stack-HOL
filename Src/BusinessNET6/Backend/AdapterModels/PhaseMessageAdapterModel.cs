using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class PhaseMessageAdapterModel : ICloneable
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
        [Required(ErrorMessage = "內容 不可為空白")]
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
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

        public int PhaseCategoryId { get; set; }
        public virtual PhaseCategoryAdapterModel PhaseCategory { get; set; }
        public string PhaseCategoryName { get; set; }

        public PhaseMessageAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as PhaseMessageAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
