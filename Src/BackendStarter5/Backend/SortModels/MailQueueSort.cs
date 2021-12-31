using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum MailQueueSortEnum
    {
        CreatedAtDescending,
        CreatedAtAscending,
    }
    public class MailQueueSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MailQueueSortEnum.CreatedAtAscending,
                Title = "建立時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MailQueueSortEnum.CreatedAtDescending,
                Title = "建立時間 遞減"
            });
        }
    }
}
