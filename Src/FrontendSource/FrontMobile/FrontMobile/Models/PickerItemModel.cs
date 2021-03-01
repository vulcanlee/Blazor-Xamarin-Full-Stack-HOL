using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FrontMobile.Models
{
    public class PickerItemModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
