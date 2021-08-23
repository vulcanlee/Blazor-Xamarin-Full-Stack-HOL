using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.AdapterModels
{
    public class AuditUserAdapterModel : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MyUserId { get; set; }
        public MyUserAdapterModel MyUser { get; set; }
        public string MyUserName { get; set; }
        public int Level { get; set; }
        public bool OnlyCC { get; set; }
        public bool Enable { get; set; }
        public int AuditMasterId { get; set; }
        public AuditMasterAdapterModel AuditMaster { get; set; }

        public AuditUserAdapterModel Clone()
        {
            return ((ICloneable)this).Clone() as AuditUserAdapterModel;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
