using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum AuditUserSortEnum
    {
        LevelAscending,
        LevelDescending,
    }
    public class AuditUserSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditUserSortEnum.LevelAscending,
                Title = "階級 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditUserSortEnum.LevelDescending,
                Title = "階級 遞減"
             });
       }
    }
}
