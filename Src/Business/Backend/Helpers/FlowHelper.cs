﻿using AutoMapper;
using Backend.AdapterModels;
using Backend.Models;
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
    public static class FlowHelper
    {
        /// <summary>
        /// 轉換狀態碼成為說明文字
        /// </summary>
        /// <param name="flowMasterAdapterModel"></param>
        /// <returns></returns>
        public static FlowMasterAdapterModel GetFlowName(this FlowMasterAdapterModel flowMasterAdapterModel)
        {
            switch (flowMasterAdapterModel.Status)
            {
                case 0:
                    flowMasterAdapterModel.StatusName = "草稿";
                    break;
                case 1:
                    flowMasterAdapterModel.StatusName = "送審";
                    break;
                case 99:
                    flowMasterAdapterModel.StatusName = "完成";
                    break;
                default:
                    flowMasterAdapterModel.StatusName = "???";
                    break;
            }
            return flowMasterAdapterModel;
        }
        public static FlowUserAdapterModel GetCompletionName(this FlowUserAdapterModel flowUserAdapterModel)
        {
            if(flowUserAdapterModel.Completion == false)
            {
                flowUserAdapterModel.CompletionName = "";
            }
            else
            {
                flowUserAdapterModel.CompletionName = "完成";
            }
            return flowUserAdapterModel;
        }
        public static FlowMasterAdapterModel GetSourceTypeName(this FlowMasterAdapterModel flowMasterAdapterModel)
        {
            if(flowMasterAdapterModel.SourceType == FlowSourceTypeEnum.None)
            {
                flowMasterAdapterModel.SourceTypeName= "無";
            }
            else if (flowMasterAdapterModel.SourceType == FlowSourceTypeEnum.WorkOrder)
            {
                flowMasterAdapterModel.SourceTypeName = "工作單";
            }
            return flowMasterAdapterModel;
        }
    }
}
