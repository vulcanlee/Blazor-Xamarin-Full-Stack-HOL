using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using BAL.Helpers;
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
                case MagicHelper.WorkOrderStatus全部:
                    result = "全部";
                    break;
                case MagicHelper.WorkOrderStatus建立:
                    result = "建立";
                    break;
                case MagicHelper.WorkOrderStatus指派人員:
                    result = "指派人員";
                    break;
                case MagicHelper.WorkOrderStatus派工:
                    result = "派工";
                    break;
                case MagicHelper.WorkOrderStatus完工:
                    result = "完工";
                    break;
                case MagicHelper.WorkOrderStatus送審:
                    result = "送審";
                    break;
                case MagicHelper.WorkOrderStatus結案:
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
