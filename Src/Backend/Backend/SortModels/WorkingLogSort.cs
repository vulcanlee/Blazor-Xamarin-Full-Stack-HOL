using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum WorkingLogSortEnum
    {
        NameAscending,
        NameDescending,
        LogDateAscending,
        LogDateDescending,
    }
    public class WorkingLogSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogSortEnum.LogDateAscending,
                Title = "日誌日期 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogSortEnum.LogDateDescending,
                Title = "日誌日期 遞減"
            });
        }
    }
}
