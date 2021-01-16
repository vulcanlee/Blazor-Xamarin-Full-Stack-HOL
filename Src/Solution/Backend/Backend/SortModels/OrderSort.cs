using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum OrderSortEnum
    {
        OrderDateAscending,
        OrderDateDescending,
    }
    public class OrderSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderSortEnum.OrderDateAscending,
                Title = "訂單時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderSortEnum.OrderDateDescending,
                Title = "訂單時間 遞減"
            });
        }
    }
}
