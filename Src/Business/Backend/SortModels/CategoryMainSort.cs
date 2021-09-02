using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum CategoryMainSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class CategoryMainSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategoryMainSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategoryMainSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
