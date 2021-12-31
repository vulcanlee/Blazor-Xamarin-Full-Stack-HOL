using CommonDomain.DataModels;
using System.Collections.Generic;

namespace Backend.SortModels
{
    public enum ExceptionRecordSortEnum
    {
        MessageAscending,
        MessageDescending,
        ExceptionTimeAscending,
        ExceptionTimeDescending,
    }
    public class ExceptionRecordSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ExceptionRecordSortEnum.MessageAscending,
                Title = "異常訊息 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ExceptionRecordSortEnum.MessageDescending,
                Title = "異常訊息 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ExceptionRecordSortEnum.ExceptionTimeAscending,
                Title = "發生時間 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)ExceptionRecordSortEnum.ExceptionTimeDescending,
                Title = "發生時間 遞減"
            });
        }
    }
}
