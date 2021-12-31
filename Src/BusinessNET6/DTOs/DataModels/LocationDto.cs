using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class LocationDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        /// <summary>
        /// 排序編號
        /// </summary>
        public int OrderNumber { get; set; }
        public string? Code { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? NFC { get; set; }
        public string? QRCode { get; set; }
        /// <summary>
        /// 緯度 
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 經度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enable { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public LocationDto Clone()
        {
            return ((ICloneable)this).Clone() as LocationDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
