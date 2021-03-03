using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum LeaveCategorySortEnum
    {
        NameAscending,
        NameDescending,
        OrderNumberAscending,
        OrderNumberDescending,
    }
    public class LeaveCategorySort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveCategorySortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveCategorySortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveCategorySortEnum.OrderNumberAscending,
                Title = "順序 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)LeaveCategorySortEnum.OrderNumberDescending,
                Title = "順序 遞減"
            });
        }
    }
}
