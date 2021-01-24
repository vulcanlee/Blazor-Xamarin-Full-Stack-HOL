using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class WorkingLogDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LogDate { get; set; }
        public decimal TotalHours { get; set; }
        public int MyUserId { get; set; }
        public string MyUserName { get; set; }
        public virtual MyUserDto MyUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WorkingLogDto Clone()
        {
            return ((ICloneable)this).Clone() as WorkingLogDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
