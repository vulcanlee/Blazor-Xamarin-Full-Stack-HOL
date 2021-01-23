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
    public class LeaveFormService : BaseWebAPI<LeaveFormDto>
    {
        public LeaveFormService()
            : base()
        {
            this.url = "/api/LeaveForm";
            this.host = LOBGlobal.APIEndPointHost;
            isCollection = true;
        }

        public async Task<APIResult> GetAsync()
        {
            isCollection = true;
            needSave = true;
            encodingType = EnctypeMethod.JSON;
            routeUrl = $"";

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);

            return mr;
        }

        public async Task<APIResult> PostAsync(LeaveFormDto item)
        {
            isCollection = false;
            needSave = false;
            encodingType = EnctypeMethod.JSON;
            routeUrl = $"";

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Post, CancellationToken.None);

            return mr;
        }

        public async Task<APIResult> PutAsync(LeaveFormDto item)
        {
            isCollection = false;
            needSave = false;
            encodingType = EnctypeMethod.JSON;
            routeUrl = $"/{item.Id}";

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Put, CancellationToken.None);

            return mr;
        }

        public async Task<APIResult> DeleteAsync(LeaveFormDto item)
        {
            isCollection = false;
            needSave = false;
            encodingType = EnctypeMethod.JSON;
            routeUrl = $"/{item.Id}";

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();

            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(item));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Delete, CancellationToken.None);

            return mr;
        }
    }
}
