using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum FlowMasterSortEnum
    {
        CreateDateDescending,
        CreateDateAscending,
    }
    public class FlowMasterSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowMasterSortEnum.CreateDateAscending,
                Title = "建立時間 遞增"
            });
             SortConditions.Add(new SortCondition()
            {
                Id = (int)FlowMasterSortEnum.CreateDateDescending,
                Title = "建立時間 遞減"
            });
       }
    }
}
