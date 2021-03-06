﻿using CommonLibrary.Helpers;
using CommonLibrary.Helpers.WebAPIs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataTransferObject.DTOs;
using Business.DataModel;

namespace Business.Services
{
    public class LeaveFormService : BaseWebAPI<LeaveFormDto>
    {
        private readonly AppStatus appStatus;

        public LeaveFormService(AppStatus appStatus)
            : base()
        {
            this.Url = "/api/LeaveForm";
            this.Host = LOBGlobal.APIEndPointHost;
            SetDefaultPersistentBehavior();
            this.appStatus = appStatus;
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = true;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }

        public async Task<APIResult> GetAsync()
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = appStatus.SystemStatus.Token;
            ApiResultIsCollection = true;
            EnctypeMethod = EnctypeMethod.JSON;
            Route = $"";
            #endregion

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }

        public async Task<APIResult> PostAsync(LeaveFormDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = appStatus.SystemStatus.Token;
            ApiResultIsCollection = false;
            EnctypeMethod = EnctypeMethod.JSON;
            Route = $"";
            #endregion

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Post, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }

        public async Task<APIResult> PutAsync(LeaveFormDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = appStatus.SystemStatus.Token;
            ApiResultIsCollection = false;
            EnctypeMethod = EnctypeMethod.JSON;
            Route = $"{item.Id}";
            #endregion

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Put, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }

        public async Task<APIResult> DeleteAsync(LeaveFormDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = appStatus.SystemStatus.Token;
            ApiResultIsCollection = false;
            EnctypeMethod = EnctypeMethod.JSON;
            Route = $"{item.Id}";
            #endregion

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Delete, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }
    }
}
