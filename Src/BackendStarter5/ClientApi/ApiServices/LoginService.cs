using ClientApi.CommonApiServices;
using DTOs.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApi.ApiServices
{
    public class LoginService : BaseWebAPI<LoginResponseDto>
    {
        public LoginService()
            : base()
        {
            this.Url = "/api/Login";
            this.Host = LOBGlobal.APIEndPointHost;
            SetDefaultPersistentBehavior();
        }

        void SetDefaultPersistentBehavior()
        {
            ApiResultIsCollection = false;
            PersistentStorage = ApiResultIsCollection ? PersistentStorage.Collection : PersistentStorage.Single;
        }

        public async Task<APIResult> PostAsync(LoginRequestDto loginRequestDTO)
        {
            EnctypeMethod = EnctypeMethod.JSON;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(loginRequestDTO));
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Post, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }
    }
}
