using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class SystemLogAdapterModel : ICloneable
    {
        public long Id { get; set; }
        public string? Category { get; set; }
        public string? LogLevel { get; set; }
        public string? Message { get; set; }
        public string? Content { get; set; }
        public string? IP { get; set; }
        public DateTime Updatetime { get; set; }

        public SystemLogAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as SystemLogAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
