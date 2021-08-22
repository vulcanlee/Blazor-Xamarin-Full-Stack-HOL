using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum PhaseCategorySortEnum
    {
        NameAscending,
        NameDescending,
    }
    public class PhaseCategorySort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PhaseCategorySortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)PhaseCategorySortEnum.NameDescending,
                Title = "名稱 遞減"
            });
        }
    }
}
