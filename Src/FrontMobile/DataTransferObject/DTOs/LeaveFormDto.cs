using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class LeaveFormDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public DateTime FormDay { get; set; }
        public TimeSpan FormTimeSpan { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime BeginDay { get; set; }
        public TimeSpan BeginTimeSpan { get; set; }
        public DateTime CompleteDate { get; set; }
        public DateTime CompleteDay { get; set; }
        public TimeSpan CompleteTimeSpan { get; set; }
        public decimal Hours { get; set; }
        [Required(ErrorMessage = "請假事由 不可為空白")]
        public string LeaveCause { get; set; }
        public string FormsStatus { get; set; }
        public string ApproveResult { get; set; }

        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int LeaveCategoryId { get; set; }
        public string LeaveCategoryName { get; set; }
        public virtual LeaveCategoryDto LeaveCategory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LeaveFormDto Clone()
        {
            var fooObject = ((ICloneable)this).Clone() as LeaveFormDto;
            #region 進行深層複製
            fooObject.LeaveCategory = ((ICloneable)fooObject.LeaveCategory)?.Clone() as LeaveCategoryDto;
            fooObject.MyUser = ((ICloneable)fooObject.MyUser)?.Clone() as MyUserDto;
            #endregion
            return fooObject;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetDate()
        {
            BeginDay = BeginDate.Date;
            BeginTimeSpan = BeginDate.TimeOfDay;
            FormDay = FormDate.Date;
            FormTimeSpan = FormDate.TimeOfDay;
            CompleteDay = CompleteDate.Date;
            CompleteTimeSpan = CompleteDate.TimeOfDay;
        }
        public void CombineDate()
        {
            BeginDate = BeginDay.Date + BeginTimeSpan;
            FormDate = FormDay.Date + FormTimeSpan;
            CompleteDate = CompleteDay.Date + CompleteTimeSpan;

            MyUser = null;
            LeaveCategory = null;
        }

        public string Validation()
        {
            StringBuilder sb = new StringBuilder();
            if (LeaveCategoryId <= 0)
            {
                sb.Append($"需要選擇一個 請假假別 {Environment.NewLine}");
            }
            if (MyUserId <= 0)
            {
                sb.Append($"需要選擇一個 申請人 {Environment.NewLine}");
            }
            if (AgentId <= 0)
            {
                sb.Append($"需要選擇一個 代理人 {Environment.NewLine}");
            }
            if (AgentId == MyUserId)
            {
                sb.Append($"申請人 與 代理人 不能是同一人 {Environment.NewLine}");
            }
            if (Hours <= 0)
            {
                sb.Append($"請假總時數不可小於 0 {Environment.NewLine}");
            }
            if (BeginDate>CompleteDate)
            {
                sb.Append($"請假開始時間不可超過請假結束時間 {Environment.NewLine}");
            }
            if (string.IsNullOrEmpty(LeaveCause))
            {
                sb.Append($"需要輸入請假事由 {Environment.NewLine}");
            }
            return sb.ToString();
        }
    }
}
