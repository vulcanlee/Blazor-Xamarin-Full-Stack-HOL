using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum TravelExpenseSortEnum
    {
        ApplyDateAscending,
        ApplyDateDescending,
    }
    public class TravelExpenseSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)TravelExpenseSortEnum.ApplyDateAscending,
                Title = "差旅日期 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)TravelExpenseSortEnum.ApplyDateDescending,
                Title = "差旅日期 遞減"
            });
        }
    }
}
