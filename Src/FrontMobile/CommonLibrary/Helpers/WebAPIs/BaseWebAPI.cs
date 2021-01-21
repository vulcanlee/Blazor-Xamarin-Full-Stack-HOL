using CommonLibrary.Helpers;
using CommonLibrary.Helpers.Storages;
using CommonLibrary.Helpers.Utilities;
using CommonLibrary.Helpers.WebAPIs;
using DataTransferObject.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.Helpers.WebAPIs
{
    /// <summary>
    /// 存取Http服務的Base Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseWebAPI<T>
    {
        #region Field
        /// <summary>
        /// WebAPI主機位置
        /// </summary>
        public string host = LOBGlobal.APIEndPointHost;

        /// <summary>
        /// WebAPI方法網址
        /// </summary>
        public string url = "";
        public EnctypeMethod encodingType;
        public string token = "";
        public string routeUrl = "";
        public bool needSave = false;
        public bool isCollection = true;
        /// <summary>
        /// 資料夾名稱
        /// </summary>
        public string 現在資料夾名稱 = "";
        public string 子資料夾名稱 = "";
        public string 最上層資料夾名稱 = "Data";

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string 資料檔案名稱 { get; set; }


        #region 系統用到的訊息字串
        public static readonly string APIInternalError = "System Exception = null, Result = null";
        #endregion

        #endregion

        // =========================================================================================================

        #region protected

        #endregion

        // =========================================================================================================

        #region Public
        /// <summary>
        /// 透過Http取得的資料，也許是一個物件，也許是List
        /// </summary>
        public List<T> Items { get; set; }
        public T SingleItem { get; set; }
        /// <summary>
        /// 此次呼叫的處理結果
        /// </summary>
        public APIResult managerResult { get; set; }

        public bool 資料加密處理 { get; set; } = false;

        #endregion

        // =========================================================================================================

        /// <summary>
        /// 建構子，經由繼承後使用反射取得類別的名稱當作，檔案名稱及WebAPI的方法名稱
        /// </summary>
        public BaseWebAPI()
        {
            SetWebAccessCondition("/api/", this.GetType().Name, "Datas", this.GetType().Name);
            encodingType = EnctypeMethod.FORMURLENCODED;
            現在資料夾名稱 = 最上層資料夾名稱;
            url = "";
            資料檔案名稱 = this.GetType().Name;
            子資料夾名稱 = 資料檔案名稱;
        }

        /// <summary>
        /// 建立存取 Web 服務的參數
        /// </summary>
        /// <param name="_url">存取服務的URL</param>
        /// <param name="_DataFileName">儲存資料的名稱</param>
        /// <param name="_DataFolderName">資料要儲存的目錄</param>
        /// <param name="_className">類別名稱</param>
        public void SetWebAccessCondition(string _url, string _DataFileName, string _DataFolderName, string _className = "")
        {
            string className = _className;

            this.url = string.Format("{0}{1}", _url, _className);
            this.資料檔案名稱 = _DataFileName;
            this.現在資料夾名稱 = _DataFolderName;
            this.managerResult = new APIResult();
        }

        /// <summary>
        /// 從網路取得相對應WebAPI的資料
        /// </summary>
        /// <param name="dic">所要傳遞的參數 Dictionary </param>
        /// <param name="httpMethod">Get Or Post</param>
        /// <returns></returns>
        protected virtual async Task<APIResult> SendAsync(Dictionary<string, string> dic, HttpMethod httpMethod,
            CancellationToken token = default(CancellationToken))
        {
            this.managerResult = new APIResult();
            APIResult mr = this.managerResult;
            string jsonPayload = "";

            //檢查網路狀態
            if (UtilityHelper.IsConnected() == false)
            {
                mr.Status = false;
                mr.Message = "無網路連線可用，請檢查網路狀態";
                return mr;
            }

            if (dic.ContainsKey(LOBGlobal.JSONDataKeyName))
            {
                jsonPayload = dic[LOBGlobal.JSONDataKeyName];
                dic.Remove(LOBGlobal.JSONDataKeyName);
            }

            HttpClientHandler handler = new HttpClientHandler();

            using (HttpClient client = new HttpClient(handler))
            {
                try
                {
                    //client.Timeout = TimeSpan.FromMinutes(3);
                    string fooQueryString = dic.ToQueryString();
                    string fooUrl = $"{host}{url}{routeUrl}" + fooQueryString;
                    UriBuilder ub = new UriBuilder(fooUrl);
                    HttpResponseMessage response = null;

                    if (string.IsNullOrEmpty(this.token) == false)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", this.token);
                    }

                    #region  Get Or Post
                    if (httpMethod == HttpMethod.Get)
                    {
                        // 使用 Get 方式來呼叫
                        response = await client.GetAsync(ub.Uri, token);
                    }
                    else if (httpMethod == HttpMethod.Post)
                    {
                        // 使用 Post 方式來呼叫
                        if (encodingType == EnctypeMethod.FORMURLENCODED)
                        {
                            // 使用 FormUrlEncoded 方式來進行傳遞資料的編碼
                            response = await client.PostAsync(ub.Uri, dic.ToFormUrlEncodedContent(), token);
                        }
                        else if (encodingType == EnctypeMethod.JSON)
                        {
                            client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                            response = await client.PostAsync(ub.Uri, new StringContent(jsonPayload, Encoding.UTF8, "application/json"), token);
                        }
                    }
                    else if (httpMethod == HttpMethod.Put)
                    {
                        // 使用 Post 方式來呼叫
                        if (encodingType == EnctypeMethod.FORMURLENCODED)
                        {
                            // 使用 FormUrlEncoded 方式來進行傳遞資料的編碼
                            response = await client.PutAsync(ub.Uri, dic.ToFormUrlEncodedContent(), token);
                        }
                        else if (encodingType == EnctypeMethod.JSON)
                        {
                            client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                            response = await client.PutAsync(ub.Uri, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                        }
                    }
                    else if (httpMethod == HttpMethod.Delete)
                    {
                        response = await client.DeleteAsync(ub.Uri, token);
                    }
                    else
                    {
                        throw new NotImplementedException("Not Found HttpMethod");
                    }
                    #endregion

                    #region Response
                    if (response != null)
                    {
                        String strResult = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode == true)
                        {
                            #region 回傳成功狀態碼
                            mr = JsonConvert.DeserializeObject<APIResult>(strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            if (mr.Status == true)
                            {
                                var fooDataString = mr.Payload.ToString();
                                if (isCollection == false)
                                {
                                    SingleItem = JsonConvert.DeserializeObject<T>(fooDataString, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                                    if (needSave == true)
                                    {
                                        if (SingleItem == null)
                                        {
                                            SingleItem = (T)Activator.CreateInstance(typeof(T));
                                        }
                                        await this.WriteToFileAsync();
                                    }
                                }
                                else
                                {
                                    Items = JsonConvert.DeserializeObject<List<T>>(fooDataString, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                                    if (needSave == true)
                                    {
                                        if (Items == null)
                                        {
                                            Items = (List<T>)Activator.CreateInstance(typeof(List<T>));
                                        }
                                        await this.WriteToFileAsync();
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            APIResult fooAPIResult = JsonConvert.DeserializeObject<APIResult>(strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            if (fooAPIResult != null)
                            {
                                mr = fooAPIResult;
                            }
                            else
                            {
                                mr.Status = false;
                                mr.HTTPStatus = (int)response.StatusCode;
                                mr.Message = string.Format("Error Code:{0}, Error Message:{1}", response.StatusCode, response.ReasonPhrase);
                            }
                        }
                    }
                    else
                    {
                        mr.Status = false;
                        mr.Message = APIInternalError;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    mr.Status = false;
                    mr.Message = ex.Message;
                }
            }

            return mr;
        }

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public virtual async Task ReadFromFileAsync()
        {
            if (isCollection == false)
            {
                SingleItem = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                Items = (List<T>)Activator.CreateInstance(typeof(List<T>));
            }

            string data = await StorageUtility.ReadFromDataFileAsync(this.現在資料夾名稱, this.資料檔案名稱);
            if (string.IsNullOrEmpty(data) == true)
            {

            }
            else
            {
                try
                {
                    if (isCollection == false)
                    {
                        this.SingleItem = JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    }
                    else
                    {
                        this.Items = JsonConvert.DeserializeObject<List<T>>(data, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public virtual async Task WriteToFileAsync()
        {
            string data = "";
            if (isCollection == false)
            {
                data = JsonConvert.SerializeObject(this.SingleItem);
            }
            else
            {
                data = JsonConvert.SerializeObject(this.Items);
            }
            await StorageUtility.WriteToDataFileAsync(this.現在資料夾名稱, this.資料檔案名稱, data);
        }

    }

    /// <summary>
    /// POST資料的時候，將要傳遞的參數，使用何種方式來進行編碼
    /// </summary>
    public enum EnctypeMethod
    {
        /// <summary>
        /// 使用 multipart/form-data 方式來進行傳遞參數的編碼
        /// </summary>
        MULTIPART,
        /// <summary>
        /// 使用 application/x-www-form-urlencoded 方式來進行傳遞參數的編碼
        /// </summary>
        FORMURLENCODED,
        XML,
        /// <summary>
        /// 使用 application/json 方式來進行傳遞參數的編碼
        /// </summary>
        JSON
    }
}
