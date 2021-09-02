using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum CategorySubSortEnum
    {
        OrderNumberAscending,
        OrderNumberDescending,
        NameAscending,
        NameDescending,
        EnableAscending,
        EnableDescending,
    }


    public class CategorySubSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.OrderNumberAscending,
                Title = "排序編號 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.OrderNumberDescending,
                Title = "排序編號 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.EnableAscending,
                Title = "啟用 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)CategorySubSortEnum.EnableDescending,
                Title = "啟用 遞減"
            });
        }
    }
}
