using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class WorkingLogDetailAdapterModel : ICloneable
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "工作項目 不可為空白")]
        public string Title { get; set; }
        [Required(ErrorMessage = "工作項目說明 不可為空白")]
        public string Summary { get; set; }
        public decimal Hours { get; set; }

        public int WorkingLogId { get; set; }
        public virtual WorkingLogAdapterModel WorkingLog { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public virtual ProjectAdapterModel Project { get; set; }

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public WorkingLogDetailAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as WorkingLogDetailAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
