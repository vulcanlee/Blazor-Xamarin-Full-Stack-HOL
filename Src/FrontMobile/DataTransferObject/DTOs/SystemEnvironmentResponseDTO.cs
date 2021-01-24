using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class SystemEnvironmentResponseDTO : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AndroidVersion { get; set; }
        public string AndroidUrl { get; set; }
        public string iOSVersion { get; set; }
        public string iOSUrl { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SystemEnvironmentResponseDTO Clone()
        {
            return ((ICloneable)this).Clone() as SystemEnvironmentResponseDTO;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
