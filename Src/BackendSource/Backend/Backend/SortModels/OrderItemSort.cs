using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum OrderItemSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class OrderItemSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderItemSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderItemSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
