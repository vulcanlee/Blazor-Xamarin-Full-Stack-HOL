using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public partial class MenuRoleDto : ICloneable, INotifyPropertyChanged
    {
        public MenuRoleDto()
        {
            MenuData = new HashSet<MenuDataDto>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<MenuDataDto> MenuData { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuRoleDto Clone()
        {
            return ((ICloneable)this).Clone() as MenuRoleDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
