﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using Entities.Models;
using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShareBusiness.Factories;
using ShareDomain.DataModels;
using ShareDomain.Enums;
using ShareBusiness.Helpers;

namespace Backend.Controllers
{
    //[Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveCategoryController : ControllerBase
    {
        private readonly ILeaveCategoryService LeaveCategoryService;
        private readonly IMapper mapper;

        #region 建構式
        public LeaveCategoryController(ILeaveCategoryService LeaveCategoryService,
            IMapper mapper)
        {
            this.LeaveCategoryService = LeaveCategoryService;
            this.mapper = mapper;
        }
        #endregion

        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LeaveCategoryDto data)
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

            LeaveCategoryAdapterModel record = mapper.Map<LeaveCategoryAdapterModel>(data);
            if (record != null)
            {
                var result = mapper.Map<LeaveCategoryDto>(record);

                #region 新增記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await LeaveCategoryService.BeforeAddCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await LeaveCategoryService.AddAsync(record);
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

            var records = await LeaveCategoryService.GetAsync(dataRequest);
            var result = mapper.Map<List<LeaveCategoryDto>>(records.Result);
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: result);
            return Ok(apiResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            APIResult apiResult;
            var record = await LeaveCategoryService.GetAsync(id);
            var result = mapper.Map<LeaveCategoryDto>(record);
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] LeaveCategoryDto data)
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

            var record = await LeaveCategoryService.GetAsync(id);
            if (record != null)
            {
                LeaveCategoryAdapterModel recordTarget = mapper.Map<LeaveCategoryAdapterModel>(data);
                recordTarget.Id = id;
                var result = mapper.Map<LeaveCategoryDto>(recordTarget);

                #region 修改記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await LeaveCategoryService.BeforeUpdateCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await LeaveCategoryService.UpdateAsync(recordTarget);
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
            var record = await LeaveCategoryService.GetAsync(id);
            var result = mapper.Map<LeaveCategoryDto>(record);
            if (record != null)
            {

                #region 刪除記錄前的紀錄完整性檢查
                VerifyRecordResult verify = await LeaveCategoryService.BeforeDeleteCheckAsync(record);
                if (verify.Success == false)
                {
                    apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                          ErrorMessageMappingHelper.Instance.GetErrorMessage(verify.MessageId),
                          payload: result);
                    return Ok(apiResult);
                }
                #endregion

                var verifyRecordResult = await LeaveCategoryService.DeleteAsync(id);
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
