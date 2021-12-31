using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class OrganizationUintDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "名稱 不可為空白")]
        public string Name { get; set; }
        public int UpOrganizationUintId { get; set; }
        public string UpOrganizationUintName { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public OrganizationUintDto Clone()
        {
            return ((ICloneable)this).Clone() as OrganizationUintDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
