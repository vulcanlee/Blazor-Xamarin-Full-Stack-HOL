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
    public class ExceptionRecordsManager : BaseWebAPI<ExceptionRecordDto>
    {
        private readonly AppStatus appStatus;

        public ExceptionRecordsManager(AppStatus appStatus)
            : base()
        {
            //資料檔案名稱 = "SampleRepository.txt";
            //this.url = "/webapplication/ntuhwebadminapi/webadministration/T0/searchDoctor";
            this.url = "/api/ExceptionRecords";
            this.host = LOBGlobal.APIEndPointHost;
            this.appStatus = appStatus;
        }


        public async Task<APIResult> PostAsync(List<ExceptionRecordDto> exceptionRecordRequestDTO, CancellationToken ctoken = default(CancellationToken))
        {
            token = appStatus.SystemStatus.Token;
            encodingType = EnctypeMethod.JSON;
            needSave = false;
            isCollection = true;
            routeUrl = $"";

            #region 要傳遞的參數
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            WebQueryDictionary dic = new WebQueryDictionary();

            // ---------------------------- 另外兩種建立 QueryString的方式
            //dic.Add(Global.getName(() => memberSignIn_QS.app), memberSignIn_QS.app);
            //dic.AddItem<string>(() => 查詢資料QueryString.strHospCode);
            //dic.Add("Price", SetMemberSignUpVM.Price.ToString());
            dic.Add(LOBGlobal.JSONDataKeyName, JsonConvert.SerializeObject(exceptionRecordRequestDTO));
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Post, ctoken);

            return mr;
        }

        public override async Task ReadFromFileAsync()
        {
            needSave = true;
            isCollection = true;
            await base.ReadFromFileAsync();
        }

        /// <summary>
        /// 將物件資料寫入到檔案中
        /// </summary>
        public override async Task WriteToFileAsync()
        {
            needSave = true;
            isCollection = true;
            await base.WriteToFileAsync();
        }
    }
}
