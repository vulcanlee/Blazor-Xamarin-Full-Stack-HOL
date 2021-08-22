using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum AuditMasterSortEnum
    {
        CreateDateDescending,
        CreateDateAscending,
    }
    public class AuditMasterSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditMasterSortEnum.CreateDateAscending,
                Title = "建立時間 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditMasterSortEnum.CreateDateDescending,
                Title = "建立時間 遞減"
            });
       }
    }
}
