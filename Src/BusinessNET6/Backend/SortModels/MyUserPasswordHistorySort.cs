using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum MyUserPasswordHistorySortEnum
    {
        ChangePasswordDatetimeDescending,
        ChangePasswordDatetimeAscending,
    }
    public class MyUserPasswordHistorySort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeAscending,
                Title = "變更時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserPasswordHistorySortEnum.ChangePasswordDatetimeDescending,
                Title = "變更時間 遞減"
            });
        }
    }
}
