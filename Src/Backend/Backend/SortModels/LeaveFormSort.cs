using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum LeaveFormSortEnum
    {
        FormDateAscending,
        FormDateDescending,
        HoursAscending,
        HoursDescending,
    }
    public class LeaveFormSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveFormSortEnum.FormDateAscending,
                Title = "開始時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveFormSortEnum.FormDateDescending,
                Title = "開始時間 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveFormSortEnum.HoursAscending,
                Title = "總時數 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveFormSortEnum.HoursDescending,
                Title = "總時數 遞減"
            });
        }
    }
}
