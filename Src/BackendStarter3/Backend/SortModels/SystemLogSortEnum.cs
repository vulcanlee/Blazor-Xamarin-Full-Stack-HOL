using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum SystemLogSortEnum
    {
        更新時間Descending,
        更新時間Ascending,
        MessageAscending,
        MessageDescending,
        CategoryAscending,
        CategoryDescending,
    }
    public class SystemLogSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.更新時間Descending,
                Title = "更新時間 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.更新時間Ascending,
                Title = "更新時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.MessageAscending,
                Title = "訊息 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.MessageDescending,
                Title = "訊息 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.CategoryAscending,
                Title = "分類 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)SystemLogSortEnum.CategoryDescending,
                Title = "分類 遞減"
            });
        }
    }
}
