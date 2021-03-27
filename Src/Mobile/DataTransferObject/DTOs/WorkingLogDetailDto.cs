using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class WorkingLogDetailDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "工作項目 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "工作項目說明 不可為空白")]
        public string Summary { get; set; }
        public decimal Hours { get; set; }

        public int WorkingLogId { get; set; }
        public virtual WorkingLogDto WorkingLog { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public virtual ProjectDto Project { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WorkingLogDetailDto Clone()
        {
            return ((ICloneable)this).Clone() as WorkingLogDetailDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        public string Validation()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(Title))
            {
                sb.Append($"需要輸入 工作項目  {Environment.NewLine}");
            }
            if (string.IsNullOrEmpty(Summary))
            {
                sb.Append($"需要輸入 工作項目說明   {Environment.NewLine}");
            }
            if (Hours <= 0)
            {
                sb.Append($"時數 不可小於 0 {Environment.NewLine}");
            }
            if (ProjectId <= 0)
            {
                sb.Append($"需要選擇一個 專案 {Environment.NewLine}");
            }
            return sb.ToString();
        }
    }
}
