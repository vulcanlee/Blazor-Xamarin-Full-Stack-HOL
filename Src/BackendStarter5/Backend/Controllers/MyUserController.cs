using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BAL.Factories;
using BAL.Helpers;
using CommonDomain.DataModels;
using CommonDomain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MyUserController : ControllerBase
    {
        private readonly IMyUserService myUserService;
        private readonly IMapper mapper;

        #region 建構式
        public MyUserController(IMyUserService myUserService,
            IMapper mapper)
        {
            this.myUserService = myUserService;
            this.mapper = mapper;
        }
        #endregion

        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MyUserDto data)
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

            MyUserAdapterModel record = mapper.Map<MyUserAdapterModel>(data);
            if (record != null)
            {
                var result = mapper.Map<MyUserDto>(record);

                #region 新增記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await myUserService.BeforeAddCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await myUserService.AddAsync(record);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    if (verifyRecordResult.MessageId == ErrorMessageEnum.客製化文字錯誤訊息)
                    {
                        if (verifyRecordResult.Exception == null)
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result);
                        }
                        else
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result,
                                exceptionMessage: verifyRecordResult.Exception.Message,
                                replaceExceptionMessage: true);
                        }
                    }
                    else
                    {
                        apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                            verifyRecordResult.MessageId, payload: result);
                    }
                }
            }
            else
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                    ErrorMessageEnum.傳送過來的資料有問題, payload: data);
            }
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

            var records = await myUserService.GetAsync(dataRequest);
            var result = mapper.Map<List<MyUserDto>>(records.Result);
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: result);
            return Ok(apiResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            APIResult apiResult;
            var record = await myUserService.GetAsync(id);
            var result = mapper.Map<MyUserDto>(record);
            if (record != null && record.Id != 0)
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MyUserDto data)
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

            var record = await myUserService.GetAsync(id);
            if (record != null && record.Id != 0)
            {
                MyUserAdapterModel recordTarget = mapper.Map<MyUserAdapterModel>(data);
                recordTarget.Id = id;
                var result = mapper.Map<MyUserDto>(recordTarget);

                #region 修改記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await myUserService.BeforeUpdateCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await myUserService.UpdateAsync(recordTarget);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    if (verifyRecordResult.MessageId == ErrorMessageEnum.客製化文字錯誤訊息)
                    {
                        if (verifyRecordResult.Exception == null)
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result);
                        }
                        else
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result,
                                exceptionMessage: verifyRecordResult.Exception.Message,
                                replaceExceptionMessage: true);
                        }
                    }
                    else
                    {
                        apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                            verifyRecordResult.MessageId, payload: result);
                    }
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
            var record = await myUserService.GetAsync(id);
            var result = mapper.Map<MyUserDto>(record);
            if (record != null && record.Id != 0)
            {

                #region 刪除記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await myUserService.BeforeDeleteCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await myUserService.DeleteAsync(id);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: null);
                }
                else
                {
                    if (verifyRecordResult.MessageId == ErrorMessageEnum.客製化文字錯誤訊息)
                    {
                        if (verifyRecordResult.Exception == null)
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result);
                        }
                        else
                        {
                            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                                verifyRecordResult.Message, payload: result,
                                exceptionMessage: verifyRecordResult.Exception.Message,
                                replaceExceptionMessage: true);
                        }
                    }
                    else
                    {
                        apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                            verifyRecordResult.MessageId, payload: result);
                    }
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
