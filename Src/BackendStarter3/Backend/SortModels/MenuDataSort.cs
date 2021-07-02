using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum MenuDataSortEnum
    {
        Default,
        NameAscending,
        NameDescending,
    }
    public class MenuDataSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MenuDataSortEnum.Default,
                Title = "預設"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MenuDataSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MenuDataSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
