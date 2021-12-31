using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Factories;
using BAL.Helpers;
using CommonDomain.DataModels;
using CommonDomain.Enums;
using DTOs.DataModels;
using Backend.AdapterModels;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    //[Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionRecordController : ControllerBase
    {
        private readonly IExceptionRecordService ExceptionRecordService;
        private readonly IMapper mapper;

        #region 建構式
        public ExceptionRecordController(IExceptionRecordService ExceptionRecordService,
            IMapper mapper)
        {
            this.ExceptionRecordService = ExceptionRecordService;
            this.mapper = mapper;
        }
        #endregion

        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExceptionRecordDto data)
        {
            APIResult apiResult;

            #region 驗證 DTO 物件的資料一致性
            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                      ErrorMessageEnum.傳送過來的資料有問題, payload: data);
                return Ok(apiResult);
            }
            #endregion

            ExceptionRecordAdapterModel record = mapper.Map<ExceptionRecordAdapterModel>(data);
            if (record != null)
            {
                var result = mapper.Map<ExceptionRecordDto>(record);

                #region 新增記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await ExceptionRecordService.BeforeAddCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await ExceptionRecordService.AddAsync(record);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status201Created,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                        ErrorMessageEnum.無法新增紀錄, payload: result);
                }
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.傳送過來的資料有問題, payload: data);
            }
            return Ok(apiResult);
        }
        [Route("Collection")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<ExceptionRecordDto> datas)
        {
            APIResult apiResult = new APIResult();

            #region 驗證 DTO 物件的資料一致性
            #endregion

            List<ExceptionRecordAdapterModel> records = mapper.Map<List<ExceptionRecordAdapterModel>>(datas);
            if (records != null && records.Count > 0)
            {
                foreach (var record in records)
                {
                    var result = mapper.Map<ExceptionRecordDto>(record);

                    #region 新增記錄前的紀錄完整性檢查
                    VerifyRecordResult verify = await ExceptionRecordService.BeforeAddCheckAsync(record);
                    //if (verify.Success == false)
                    //{
                    //    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    //          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                    //          payload: result);
                    //    return Ok(apiResult);
                    //}
                    #endregion

                    var verifyRecordResult = await ExceptionRecordService.AddAsync(record);
                    if (verifyRecordResult.Success)
                    {
                        apiResult = APIResultFactory.Build(true, StatusCodes.Status201Created,
                            ErrorMessageEnum.None, payload: null);
                    }
                    else
                    {
                        apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                            ErrorMessageEnum.無法新增紀錄, payload: result);
                    }
                }
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.傳送過來的資料有問題, payload: null);
            }
            apiResult = APIResultFactory.Build(true, StatusCodes.Status201Created,
                ErrorMessageEnum.None, payload: datas);
            return Ok(apiResult);
        }
        #endregion

        #region R 查詢
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            APIResult apiResult;

            #region 建立查詢物件
            DataRequest dataRequest = new DataRequest()
            {
                Skip = 0,
                Take = 0,
                Search = "",
                Sorted = null,
            };
            #endregion

            var records = await ExceptionRecordService.GetAsync(dataRequest);
            var result = mapper.Map<List<ExceptionRecordDto>>(records.Result);
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: result);
            return Ok(apiResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            APIResult apiResult;
            var record = await ExceptionRecordService.GetAsync(id);
            var result = mapper.Map<ExceptionRecordDto>(record);
            if (record != null)
            {
                apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                    ErrorMessageEnum.None, payload: result);
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.沒有任何符合資料存在, payload: result);
            }

            return Ok(apiResult);
        }
        #endregion

        #region U 更新
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ExceptionRecordDto data)
        {
            APIResult apiResult;

            #region 驗證 DTO 物件的資料一致性
            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                      ErrorMessageEnum.傳送過來的資料有問題, payload: data);
                return Ok(apiResult);
            }
            #endregion

            var record = await ExceptionRecordService.GetAsync(id);
            if (record != null)
            {
                ExceptionRecordAdapterModel recordTarget = mapper.Map<ExceptionRecordAdapterModel>(data);
                recordTarget.Id = id;
                var result = mapper.Map<ExceptionRecordDto>(recordTarget);

                #region 修改記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await ExceptionRecordService.BeforeUpdateCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await ExceptionRecordService.UpdateAsync(recordTarget);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                        ErrorMessageEnum.無法修改紀錄, payload: result);
                }
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.沒有任何符合資料存在, payload: data);
            }
            return Ok(apiResult);
        }
        #endregion

        #region D 刪除
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            APIResult apiResult;
            var record = await ExceptionRecordService.GetAsync(id);
            var result = mapper.Map<ExceptionRecordDto>(record);
            if (record != null)
            {

                #region 刪除記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await ExceptionRecordService.BeforeDeleteCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await ExceptionRecordService.DeleteAsync(id);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                        ErrorMessageEnum.無法刪除紀錄, payload: result);
                }
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.沒有任何符合資料存在, payload: result);
            }
            return Ok(apiResult);
        }
        #endregion

    }
}
