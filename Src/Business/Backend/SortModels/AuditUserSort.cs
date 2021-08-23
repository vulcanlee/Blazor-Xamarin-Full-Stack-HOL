using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum AuditUserSortEnum
    {
        NameDescending,
        NameAscending,
    }
    public class AuditUserSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditUserSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditUserSortEnum.NameDescending,
                Title = "名稱 遞減"
             });
       }
    }
}
