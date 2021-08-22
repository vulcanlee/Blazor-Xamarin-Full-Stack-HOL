using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum PhaseMessageSortEnum
    {
        OrderNumberAscending,
        OrderNumberDescending,
    }
    public class PhaseMessageSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PhaseMessageSortEnum.OrderNumberAscending,
                Title = "排序編號 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PhaseMessageSortEnum.OrderNumberDescending,
                Title = "排序編號 遞減"
            });
        }
    }
}
