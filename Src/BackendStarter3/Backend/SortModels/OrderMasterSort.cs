using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum OrderMasterSortEnum
    {
        OrderDateAscending,
        OrderDateDescending,
    }
    public class OrderMasterSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderMasterSortEnum.OrderDateAscending,
                Title = "訂單時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OrderMasterSortEnum.OrderDateDescending,
                Title = "訂單時間 遞減"
            });
        }
    }
}
