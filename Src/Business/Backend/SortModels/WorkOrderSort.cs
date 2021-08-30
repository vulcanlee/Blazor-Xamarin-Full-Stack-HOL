using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum WorkOrderSortEnum
    {
        CreatedAtDescending,
        CreatedAtAscending,
        StartDateDescending,
        StartDateAscending,
        StatusAscending,
        StatusDescending,
    }
    public class WorkOrderSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.CreatedAtAscending,
                Title = "建立時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.CreatedAtDescending,
                Title = "建立時間 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.StartDateAscending,
                Title = "開始時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.StartDateDescending,
                Title = "開始時間 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.StatusAscending,
                Title = "狀態 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)WorkOrderSortEnum.StatusDescending,
                Title = "狀態 遞減"
            });
        }
    }
}
