using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum WorkingLogDetailSortEnum
    {
        TitleAscending,
        TitleDescending,
        HoursAscending,
        HoursDescending,
    }
    public class WorkingLogDetailSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogDetailSortEnum.TitleAscending,
                Title = "工作項目 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogDetailSortEnum.TitleDescending,
                Title = "工作項目 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogDetailSortEnum.HoursAscending,
                Title = "工作時數 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkingLogDetailSortEnum.HoursDescending,
                Title = "工作時數 遞減"
            });
        }
    }
}
