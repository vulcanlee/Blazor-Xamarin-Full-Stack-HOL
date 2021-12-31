using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public partial class GroupMemberDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int GroupMasterId { get; set; }
        public virtual GroupMasterDto GroupMaster { get; set; }
        public string GroupMasterName { get; set; }
        public int MyUserId { get; set; }
        public virtual MyUserDto MyUser { get; set; }
        public string MyUserName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public GroupMemberDto Clone()
        {
            return ((ICloneable)this).Clone() as GroupMemberDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
