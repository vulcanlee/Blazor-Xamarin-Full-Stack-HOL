using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class PolicyDetailAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        public int PolicyHeaderId { get; set; }
        public PolicyHeaderAdapterModel PolicyHeader { get; set; }
        public string PolicyHeaderName { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
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

        public PolicyDetailAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as PolicyDetailAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
