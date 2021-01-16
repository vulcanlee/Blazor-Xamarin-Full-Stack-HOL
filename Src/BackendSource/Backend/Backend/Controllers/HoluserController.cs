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
    [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "Administrator")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HoluserController : ControllerBase
    {
        private readonly IHoluserService holuserService;
        private readonly IMapper mapper;

        #region 建構式
        public HoluserController(IHoluserService holuserService,
            IMapper mapper)
        {
            this.holuserService = holuserService;
            this.mapper = mapper;
        }
        #endregion

        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HoluserDto data)
        {
            APIResult apiResult;
            HoluserAdapterModel record = mapper.Map<HoluserAdapterModel>(data);
            if (record != null)
            {
                var result = mapper.Map<HoluserDto>(record);
                var verifyRecordResult = await holuserService.AddAsync(record);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                        ErrorMessageEnum.None, payload: result);
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

            var records = await holuserService.GetAsync(dataRequest);
            var result = mapper.Map<List<HoluserDto>>(records);
            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: result);
            return Ok(apiResult);
        }
      
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            APIResult apiResult;
            var record = await holuserService.GetAsync(id);
            var result = mapper.Map<HoluserDto>(record);
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
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] HoluserDto data)
        {
            APIResult apiResult;
            var record = await holuserService.GetAsync(id);
            if (record != null)
            {
                HoluserAdapterModel recordTarget = mapper.Map<HoluserAdapterModel>(data);
                recordTarget.Id = id;
                var result = mapper.Map<HoluserDto>(recordTarget);
                var verifyRecordResult = await holuserService.UpdateAsync(recordTarget);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: result);
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
            var record = await holuserService.GetAsync(id);
            var result = mapper.Map<HoluserDto>(record);
            if (record != null)
            {
                var verifyRecordResult = await holuserService.DeleteAsync(id);
                if (verifyRecordResult.Success)
                {
                    apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                        ErrorMessageEnum.None, payload: result);
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
