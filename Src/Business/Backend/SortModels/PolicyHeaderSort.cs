using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum PolicyHeaderSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class PolicyHeaderSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyHeaderSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyHeaderSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
