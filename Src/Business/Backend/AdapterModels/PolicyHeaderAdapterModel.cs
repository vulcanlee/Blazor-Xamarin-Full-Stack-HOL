using BAL.Helpers;
using System;

namespace Backend.AdapterModels
{
    public class PolicyHeaderAdapterModel : ICloneable
    {
        public int Id { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
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
