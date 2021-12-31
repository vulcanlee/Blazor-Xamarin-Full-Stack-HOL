using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class GroupMasterDto : ICloneable, INotifyPropertyChanged
    {
        public virtual ICollection<GroupMemberDto> GroupMember { get; set; }
        public GroupMasterDto()
        {
            GroupMember = new HashSet<GroupMemberDto>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public GroupMasterDto Clone()
        {
            return ((ICloneable)this).Clone() as GroupMasterDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
