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
    public class LeaveCategoryManager : BaseWebAPI<LeaveCategoryDto>
    {
        public LeaveCategoryManager()
            : base()
        {
            this.url = "/api/LeaveCategory";
            this.host = LOBGlobal.APIEndPointHost;
            isCollection = true;
        }

        public async Task<APIResult> GetAsync()
        {
            isCollection = true;
            needSave = true;
            encodingType = EnctypeMethod.JSON;

            #region 要傳遞的參數
            WebQueryDictionary dic = new WebQueryDictionary();
            #endregion

            var mr = await this.SendAsync(dic, HttpMethod.Get, CancellationToken.None);

            return mr;
        }
    }
}
