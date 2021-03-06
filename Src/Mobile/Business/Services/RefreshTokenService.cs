﻿using Business.DataModel;
using CommonLibrary.Helpers;
using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RefreshTokenService : BaseWebAPI<LoginResponseDto>
    {
        private readonly AppStatus appStatus;

        public RefreshTokenService(AppStatus appStatus)
            : base()
        {
            this.Url = "/api/Login/RefreshToken";
            this.Host = LOBGlobal.APIEndPointHost;
            SetDefaultPersistentBehavior();
            this.appStatus = appStatus;
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = false;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }

        public async Task<APIResult> GetAsync()
        {
            Token = appStatus.SystemStatus.RefreshToken;
            EnctypeMethod = EnctypeMethod.JSON;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            //dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(loginRequestDTO));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }
    }
}
