using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum ProductSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class ProductSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ProductSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ProductSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
