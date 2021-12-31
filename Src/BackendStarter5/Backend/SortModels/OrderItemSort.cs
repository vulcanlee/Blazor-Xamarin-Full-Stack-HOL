using CommonDomain.DataModels;
using System.Collections.Generic;

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
