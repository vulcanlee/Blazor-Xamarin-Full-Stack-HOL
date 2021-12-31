using BAL.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class ExceptionRecordAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public int? MyUserId { get; set; }
        public string MyUserName { get; set; } = "";

        public string? DeviceName { get; set; }
        public string? DeviceModel { get; set; }
        public string? OSType { get; set; }
        public string? OSVersion { get; set; }
        public string? Message { get; set; }
        public string? CallStack { get; set; }
        public DateTime ExceptionTime { get; set; }

        public MyUserAdapterModel? User { get; set; }

        public bool IsExist()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ExceptionRecordAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as ExceptionRecordAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
