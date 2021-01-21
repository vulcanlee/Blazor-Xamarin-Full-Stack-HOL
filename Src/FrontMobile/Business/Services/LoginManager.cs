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
    public class LoginManager : BaseWebAPI<LoginResponseDto>
    {
        public LoginManager()
            : base()
        {
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            this.url = "/api/Login";
            this.host = LOBGlobal.APIEndPointHost;
            isCollection = false;
        }

        public async Task<APIResult> PostAsync(LoginRequestDto loginRequestDTO)
        {
            encodingType = EnctypeMethod.JSON;

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(loginRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Post, CancellationToken.None);

            return mr;
        }
    }
}
