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
using System;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CRUDDto data)
        {
            APIResult apiResult;

            #region 驗證 DTO 物件的資料一致性
            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                      "傳送過來的資料有問題", payload: null);
                return Ok(apiResult);
            }
            #endregion
            #region 新增紀錄
            await Task.Yield();
            data.Id = new Random().Next(1, 99999);
            #endregion
            apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: data);
            return Ok(apiResult);
        }
        #endregion

        #region R 查詢
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            APIResult apiResult;

            #region 查詢物件
            await Task.Yield();
            List<CRUDDto> CRUDDtos = new()
            {
                new CRUDDto() { Id = 1, Name = "CRUD1", Price = 100, Updatetime = DateTime.Now },
                new CRUDDto() { Id = 2, Name = "CRUD2", Price = 200, Updatetime = DateTime.Now },
                new CRUDDto() { Id = 3, Name = "CRUD3", Price = 300, Updatetime = DateTime.Now },
                new CRUDDto() { Id = 3, Name = "CRUD4", Price = 400, Updatetime = DateTime.Now },
            };
            #endregion

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: CRUDDtos);
            return Ok(apiResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            APIResult apiResult;
            #region 查詢特定物件
            await Task.Yield();
            CRUDDto CRUDDto = new() { Id = id, Name = $"CRUD{id}", Price = 300, Updatetime = DateTime.Now };
            #endregion
                apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: CRUDDto);
            return Ok(apiResult);
        }
        #endregion

        #region U 更新
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CRUDDto data)
        {
            APIResult apiResult;

            #region 驗證 DTO 物件的資料一致性
            if (!ModelState.IsValid)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                      "傳送過來的資料有問題", payload: null);
                return Ok(apiResult);
            }
            #endregion
            #region 更新物件
            await Task.Yield();
            CRUDDto CRUDDto = data;
            #endregion
            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                ErrorMessageEnum.None, payload: null);
            return Ok(apiResult);
        }
        #endregion

        #region D 刪除
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            APIResult apiResult;
            #region 刪除物件
            await Task.Yield();
            #endregion
            apiResult = APIResultFactory.Build(true, StatusCodes.Status202Accepted,
                ErrorMessageEnum.None, payload: null);
            return Ok(apiResult);
        }
        #endregion

    }
}
