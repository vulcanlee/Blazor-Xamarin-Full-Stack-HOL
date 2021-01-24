using CommonLibrary.Helpers;
using CommonLibrary.Helpers.WebAPIs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataTransferObject.DTOs;

namespace Business.Services
{
    public class LeaveCategoryService : BaseWebAPI<LeaveCategoryDto>
    {
        public LeaveCategoryService()
            : base()
        {
            this.Url = "/api/LeaveCategory";
            this.Host = LOBGlobal.APIEndPointHost;
            SetDefaultPersistentBehavior();
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = true;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }

        public async Task<APIResult> GetAsync()
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = "";
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
    }
}
