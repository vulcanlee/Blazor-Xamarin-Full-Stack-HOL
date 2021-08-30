using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using Domains.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class WorkOrderStatusHelper
    {
        /// <summary>
        /// 轉換狀態碼成為說明文字
        /// </summary>
        /// <param name="flowMasterAdapterModel"></param>
        /// <returns></returns>
        public static string GetWorkOrderStatus(this int workOrderStatus)
        {
            string result = "N/A";
            switch (workOrderStatus)
            {
                case -1:
                    result = "全部";
                    break;
                case 0:
                    result = "建立";
                    break;
                case 1:
                    result = "指派人員";
                    break;
                case 2:
                    result = "派工";
                    break;
                case 3:
                    result = "完工";
                    break;
                case 4:
                    result = "送審";
                    break;
                case 99:
                    result = "結案";
                    break;
                default:
                    result = "N/A";
                    break;
            }
            return result;
        }
    }
}
