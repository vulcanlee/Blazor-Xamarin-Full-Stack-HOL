﻿using ShareDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.SortModels
{
    public enum MyUserSortEnum
    {
        NameAscending,
        NameDescending,
        AccountAscending,
        AccountDescending,
        DepartmentNameAscending,
        DepartmentNameDescending,
    }
    public class MyUserSort
    {
        public static void Initialization(List<SortCondition> SortConditions)
        {
            SortConditions.Clear();
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.NameAscending,
                Title = "名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.NameDescending,
                Title = "名稱 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.AccountAscending,
                Title = "帳號 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.AccountDescending,
                Title = "帳號 遞減"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.DepartmentNameAscending,
                Title = "部門名稱 遞增"
            });
            SortConditions.Add(new SortCondition()
            {
                Id = (int)MyUserSortEnum.DepartmentNameDescending,
                Title = "部門名稱 遞減"
            });
        }
    }
}
