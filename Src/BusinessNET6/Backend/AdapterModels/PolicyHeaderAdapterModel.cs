using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class PolicyHeaderAdapterModel : ICloneable
    {
        public int Id { get; set; }
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
        /// <summary>
        /// 啟用電子郵件通知
        /// </summary>
        public bool EnableEmailNotification { get; set; }

        public PolicyHeaderAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as PolicyHeaderAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
