using ClientApi.CommonApiServices;
using DTOs.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApi.ApiServices
{
    public class ProductService : BaseWebAPI<ProductDto>
    {

        public ProductService()
            : base()
        {
            this.Url = "/api/Product";
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
            Token = LOBGlobal.AccessToken;
            ApiResultIsCollection = true;
            EnctypeMethod = EnctypeMethod.None;
            Route = $"";
            #endregion

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();
            #endregion

            APIResult apiResult = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);
            SetDefaultPersistentBehavior();

            return apiResult;
        }

        public async Task<APIResult> PostAsync(ProductDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = LOBGlobal.AccessToken;
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

        public async Task<APIResult> PutAsync(ProductDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = LOBGlobal.AccessToken;
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

        public async Task<APIResult> DeleteAsync(ProductDto item)
        {
            #region 指定此次呼叫 Web API 要執行參數
            Token = LOBGlobal.AccessToken;
            ApiResultIsCollection = false;
            EnctypeMethod = EnctypeMethod.None;
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
