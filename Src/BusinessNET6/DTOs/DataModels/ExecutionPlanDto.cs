using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class ExecutionPlanDto : ICloneable, INotifyPropertyChanged
    {
        public virtual ICollection<ExecutionFormDto> ExecutionForm { get; set; }
        public ExecutionPlanDto()
        {
            ExecutionForm = new HashSet<ExecutionFormDto>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 執行計畫名稱
        /// </summary>
        [Required(ErrorMessage = "執行計畫名稱 不可為空白")]
        public string Name { get; set; }
        /// <summary>
        /// 執行計畫說明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 執行計畫備註
        /// </summary>
        public string Remark { get; set; }
        // 計畫開始日期
        public DateTime BeginDay { get; set; }
        // 計畫結束日期
        public DateTime EndDay { get; set; }
        // 啟用
        public bool Enable { get; set; }
        public int GroupMasterId { get; set; }
        public GroupMasterDto GroupMaster { get; set; }
        public string GroupMasterName { get; set; }
        public int OrganizationUintId { get; set; }
        public OrganizationUintDto OrganizationUint { get; set; }
        public string OrganizationUintName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public ExecutionPlanDto Clone()
        {
            return ((ICloneable)this).Clone() as ExecutionPlanDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
