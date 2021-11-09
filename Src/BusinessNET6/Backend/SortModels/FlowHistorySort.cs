using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum FlowHistorySortEnum
    {
        CreateDateDescending,
        CreateDateAscending,
    }
    public class FlowHistorySort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowHistorySortEnum.CreateDateAscending,
                Title = "時間 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowHistorySortEnum.CreateDateDescending,
                Title = "時間 遞減"
            });
       }
    }
}
