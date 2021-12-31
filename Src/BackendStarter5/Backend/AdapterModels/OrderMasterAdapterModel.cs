using System;

namespace Backend.AdapterModels
{
    public class OrderMasterAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public OrderMasterAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as OrderMasterAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
