using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum FlowInboxSortEnum
    {
        CreateDateDescending,
        CreateDateAscending,
    }
    public class FlowInboxSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowInboxSortEnum.CreateDateAscending,
                Title = "時間 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowInboxSortEnum.CreateDateDescending,
                Title = "時間 遞減"
            });
       }
    }
}
