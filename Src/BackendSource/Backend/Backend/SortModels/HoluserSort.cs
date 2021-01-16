using ShareDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum HoluserSortEnum
    {
        NameAscending,
        NameDescending,
        AccountAscending,
        AccountDescending,
    }
    public class HoluserSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)HoluserSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)HoluserSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)HoluserSortEnum.AccountAscending,
                Title = "帳號 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)HoluserSortEnum.AccountDescending,
                Title = "帳號 遞減"
            });
        }
    }
}
