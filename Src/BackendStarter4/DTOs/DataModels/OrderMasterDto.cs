using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public partial class OrderMasterDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public OrderMasterDto Clone()
        {
            return ((ICloneable)this).Clone() as OrderMasterDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
