using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum MenuRoleSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class MenuRoleSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MenuRoleSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MenuRoleSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
