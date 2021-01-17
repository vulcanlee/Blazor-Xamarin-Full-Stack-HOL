using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum ProjectSortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class ProjectSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ProjectSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ProjectSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
