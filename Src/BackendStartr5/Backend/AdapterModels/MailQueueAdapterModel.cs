using CommonDomain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Backend.Helpers;

namespace Backend.AdapterModels
{
    public class MailQueueAdapterModel : ICloneable
    {
        public long Id { get; set; }
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
        public string To { get; set; } = String.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? SendedAt { get; set; }
        public int Status { get; set; }
        public string StatusName
        {
            get
            {
                return ToDescriptipnHelper.MailQueueStatusName(Status);
            }
        }
        public int SendTimes { get; set; }

        public MailQueueAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as MailQueueAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
