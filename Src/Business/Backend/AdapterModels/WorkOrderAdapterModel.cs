using CommonDomain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class WorkOrderAdapterModel : ICloneable
    {
        public int Id { get; set; }
        /// <summary>
        /// 問題描述
        /// </summary>
        [Required(ErrorMessage = "需求描述 不可為空白")]
        public string Description { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public WorkOrderStatusEnum Status { get; set; }

        public WorkOrderAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as WorkOrderAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
