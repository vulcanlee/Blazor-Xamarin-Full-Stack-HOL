using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum FlowUserSortEnum
    {
        LevelAscending,
        LevelDescending,
    }
    public class FlowUserSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowUserSortEnum.LevelAscending,
                Title = "階級 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowUserSortEnum.LevelDescending,
                Title = "階級 遞減"
             });
       }
    }
}
