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
    public class OnlyUserManager : BaseWebAPI<OnlyUserDto>
    {
        private readonly AppStatus appStatus;

        public OnlyUserManager(AppStatus appStatus)
            : base()
        {
            this.url = "/api/OnlyUser";
            this.host = LOBGlobal.APIEndPointHost;
            isCollection = false;
            this.appStatus = appStatus;
        }

        public async Task<APIResult> GetAsync()
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            //dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(loginRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);

            return mr;
        }
    }
}
