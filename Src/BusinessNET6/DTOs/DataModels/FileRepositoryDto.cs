using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTOs.DataModels
{
    public class FileRepositoryDto : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Filename { get; set; }
        public string FileExtension { get; set; }
        public DateTime CreateAt { get; set; }
        /// <summary>
        /// 該檔案的使用來源資料表
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 該檔案的使用來源資料表紀錄 Id
        /// </summary>
        public int ReferenceId { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public FileRepositoryDto Clone()
        {
            return ((ICloneable)this).Clone() as FileRepositoryDto;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
