using System;

namespace Backend.AdapterModels
{
    public class OrderAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public OrderAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as OrderAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
