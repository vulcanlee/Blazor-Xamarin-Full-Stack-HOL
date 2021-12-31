using DTOs.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    /// <summary>
    /// 存取Http服務的Base Class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseWebAPI<T>
    {
        #region Property
        /// <summary>
        /// WebAPI主機位置
        /// </summary>
        public string Host { get; set; } = LOBGlobal.APIEndPointHost;

        /// <summary>
        /// WebAPI方法網址
        /// </summary>
        public string Url { get; set; } = "";
        /// <summary>
        /// 指定 HTTP 執行的方法
        /// </summary>
        public EnctypeMethod EnctypeMethod { get; set; }
        /// <summary>
        /// 指定需要有授權限制的存取權杖
        /// </summary>
        public string Token { get; set; } = "";
        /// <summary>
        /// 其他路由參數(可用於標準 CRUD 的修改與刪除)
        /// </summary>
        public string Route { get; set; } = "";
        /// <summary>
        /// 呼叫 API 結果是集合物件或者是單一物件
        /// </summary>
        public bool ApiResultIsCollection { get; set; } = true;
        /// <summary>
        /// 儲存資料要以集合或者單一形式來儲存
        /// </summary>
        public PersistentStorage PersistentStorage { get; set; } = PersistentStorage.Collection;
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
        public APIResult ServiceResult { get; set; }

        public bool 資料加密處理 { get; set; } = false;

        #endregion

        // =========================================================================================================

        /// <summary>
        /// 建構子，經由繼承後使用反射取得類別的名稱當作，檔案名稱及WebAPI的方法名稱
        /// </summary>
        public BaseWebAPI()
        {
            SetWebAccessCondition("/api/", this.GetType().Name, "Datas", this.GetType().Name);
            EnctypeMethod = EnctypeMethod.FORMURLENCODED;
            現在資料夾名稱 = 最上層資料夾名稱;
            Url = "";
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

            this.Url = string.Format("{0}{1}", _url, _className);
            this.資料檔案名稱 = _DataFileName;
            this.現在資料夾名稱 = _DataFolderName;
            this.ServiceResult = new APIResult();
        }

        /// <summary>
        /// 從網路取得相對應WebAPI的資料
        /// </summary>
        /// <param name="dic">所要傳遞的參數 Dictionary </param>
        /// <param name="httpMethod">Get Or Post</param>
        /// <returns></returns>
        protected virtual async Task<APIResult> SendAsync(Dictionary<string, string> dic, HttpMethod httpMethod,
            CancellationToken cancellationTokentoken = default(CancellationToken))
        {
            this.ServiceResult = new APIResult();
            APIResult apiResult = this.ServiceResult;
            string jsonPayload = "";
            string queryString = "";
            string endPoint = "";
            HttpRequestMessage request = null;
            HttpResponseMessage response = null;

            #region 確認網路已經連線
            #endregion

            if (dic.ContainsKey(LOBGlobal.JSONDataKeyName))
            {
                jsonPayload = dic[LOBGlobal.JSONDataKeyName];
                dic.Remove(LOBGlobal.JSONDataKeyName);
            }

            HttpClientHandler handler = new HttpClientHandler();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //client.Timeout = TimeSpan.FromSeconds(5);
                    queryString = dic.ToQueryString();
                    client.BaseAddress = new Uri(Host);
                    endPoint = $"{Url}/{Route}" + queryString;

                    #region 填入存取權杖物件
                    if (string.IsNullOrEmpty(this.Token) == false)
                    {
                        client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", this.Token);
                    }
                    #endregion

                    #region 根據 HttpMethod 來建立相關 Request 與 HttpContent
                    request = new HttpRequestMessage(httpMethod, endPoint);
                    if (EnctypeMethod == EnctypeMethod.FORMURLENCODED)
                    {
                        request.Content = dic.ToFormUrlEncodedContent();
                    }
                    else if (EnctypeMethod == EnctypeMethod.JSON)
                    {
                        client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    }
                    #endregion

                    #region Response
                    response = await client.SendAsync(request, cancellationTokentoken);

                    if (response != null)
                    {
                        String strResult = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode == true)
                        {
                            #region 回傳成功狀態碼
                            apiResult = JsonConvert.DeserializeObject<APIResult>(strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            if (apiResult.Status == true)
                            {
                                var fooDataString = apiResult.Payload == null ? "" : apiResult.Payload.ToString();
                                if (ApiResultIsCollection == false)
                                {
                                    SingleItem = JsonConvert.DeserializeObject<T>(fooDataString, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                                    if (ApiResultIsCollection == false && PersistentStorage == PersistentStorage.Single)
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
                                    if (ApiResultIsCollection == true && PersistentStorage == PersistentStorage.Collection)
                                    {
                                        if (Items == null)
                                        {
                                            Items = (List<T>)Activator.CreateInstance(typeof(List<T>));
                                        }
                                        await this.WriteToFileAsync();
                                    }
                                }
                            }
                            else
                            {
                                apiResult.Status = false;
                                apiResult.HTTPStatus = (int)response.StatusCode;
                                apiResult.Message = $"{apiResult.Message}";
                            }
                            #endregion
                        }
                        else
                        {
                            APIResult fooAPIResult = JsonConvert.DeserializeObject<APIResult>(strResult, new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore });
                            if (fooAPIResult != null)
                            {
                                apiResult = fooAPIResult;
                                if (apiResult.Status == true)
                                {
                                    apiResult.Status = false;
                                    apiResult.Message = strResult;
                                }
                            }
                            else
                            {
                                apiResult.Status = false;
                                apiResult.HTTPStatus = (int)response.StatusCode;
                                apiResult.Message = string.Format("Error Code:{0}, Error Message:{1}", response.StatusCode, response.ReasonPhrase);
                            }
                        }
                    }
                    else
                    {
                        apiResult.Status = false;
                        apiResult.Message = APIInternalError;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    apiResult.Status = false;
                    apiResult.Message = ex.Message;
                }
            }

            return apiResult;
        }

        /// <summary>
        /// 將物件資料從檔案中讀取出來
        /// </summary>
        public virtual async Task ReadFromFileAsync()
        {
            if (PersistentStorage == PersistentStorage.Single)
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
                    if (PersistentStorage == PersistentStorage.Single)
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
                    Console.WriteLine(ex.Message);
                }
            }

        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public virtual async Task WriteToFileAsync()
        {
            string data = "";
            if (PersistentStorage == PersistentStorage.Single)
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
    /// 儲存資料的型態
    /// </summary>
    public enum PersistentStorage
    {
        /// <summary>
        /// 集合型態的物件
        /// </summary>
        Collection,
        /// <summary>
        /// 單一型態的物件
        /// </summary>
        Single
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
        JSON,
        None,
    }
}
