using ShareDomain;
using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum OnCallPhoneSortEnum
    {
        TitleAscending,
        TitleDescending,
        PhoneNumberAscending,
        PhoneNumberDescending,
        OrderNumberAscending,
        OrderNumberDescending,
    }
    public class OnCallPhoneSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.TitleAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.TitleDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.PhoneNumberAscending,
                Title = "電話號碼 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.PhoneNumberDescending,
                Title = "電話號碼 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.OrderNumberAscending,
                Title = "順序 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)OnCallPhoneSortEnum.OrderNumberDescending,
                Title = "順序 遞減"
            });
        }
    }
}
