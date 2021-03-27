using Business.DataModel;
using CommonLibrary.Helpers;
using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ExceptionRecordsService : BaseWebAPI<ExceptionRecordDto>
    {
        private readonly AppStatus appStatus;

        public ExceptionRecordsService(AppStatus appStatus)
            : base()
        {
            this.Url = "/api/ExceptionRecord";
            this.Host = LOBGlobal.APIEndPointHost;
            SetDefaultPersistentBehavior();
            this.appStatus = appStatus;
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = true;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }

        public async Task<APIResult> PostAsync(List<ExceptionRecordDto> exceptionRecordRequestDTO, CancellationToken ctoken = default(CancellationToken))
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = appStatus.SystemStatus.Token;
            EnctypeMethod = EnctypeMethod.JSON;
            ApiResultIsCollection = true;
            Route = $"Collection";
            #endregion

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(exceptionRecordRequestDTO));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Post, ctoken);
            SetDefaultPersistentBehavior();

            return apiResult;
        }
    }
}
