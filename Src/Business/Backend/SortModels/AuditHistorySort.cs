using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum AuditHistorySortEnum
    {
        CreateDateDescending,
        CreateDateAscending,
    }
    public class AuditHistorySort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditHistorySortEnum.CreateDateAscending,
                Title = "時間 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)AuditHistorySortEnum.CreateDateDescending,
                Title = "時間 遞減"
            });
       }
    }
}
