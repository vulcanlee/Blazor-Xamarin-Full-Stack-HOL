using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum PolicyDetailSortEnum
    {
        LevelAscending,
        LevelDescending,
        NameAscending,
        NameDescending,
        EnableAscending,
        EnableDescending,
    }


    public class PolicyDetailSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.LevelAscending,
                Title = "階層 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.LevelDescending,
                Title = "階層 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.EnableAscending,
                Title = "啟用 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PolicyDetailSortEnum.EnableDescending,
                Title = "啟用 遞減"
            });
        }
    }
}
