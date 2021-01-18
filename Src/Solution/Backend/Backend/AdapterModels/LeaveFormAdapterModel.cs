using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class LeaveFormAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public decimal Hours { get; set; }
        [Required(ErrorMessage = "請假事由 不可為空白")]
        public string LeaveCause { get; set; }
        public string FormsStatus { get; set; }
        public string ApproveResult { get; set; }

        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserAdapterModel MyUser { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int LeaveCategoryId { get; set; }
        public string LeaveCategoryName { get; set; }
        public virtual LeaveCategoryAdapterModel LeaveCategory { get; set; }

        public bool IsExist()
        {
            if (FormDate == default(DateTime))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public LeaveFormAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as LeaveFormAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
