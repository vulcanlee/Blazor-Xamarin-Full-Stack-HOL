using Backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Backend.Models;
using System.IO;
using DTOs.DataModels;
using CommonDomain.Enums;
using Newtonsoft.Json;

namespace Backend.Middlewares
{
    /// <summary>
    /// 當呼叫 API ( /api/someController ) 且該服務端點不存在的時候，將會替換網頁為 404 的 APIResult 訊息
    /// </summary>
    public class ApiNotFoundPageToApiResultMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiNotFoundPageToApiResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context, BlazorAppContext blazorAppContext)
        {
            string responseContent;
            StreamWriter streamWriter = null;
            string keyUrl = "/api/";
            var originalBodyStream = context.Response.Body;
            using (var fakeResponseBody = new MemoryStream())
            {
                context.Response.Body = fakeResponseBody;

                await _next(context);

                if (context.Request.Path.HasValue)
                {
                    if (context.Request.Path.Value.Length > keyUrl.Length &&
                        context.Request.Path.Value.ToLower().Substring(0, keyUrl.Length) == keyUrl)
                    {
                        fakeResponseBody.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(fakeResponseBody);
                        responseContent = await reader.ReadToEndAsync();
                        if (responseContent.Contains("<!DOCTYPE html>"))
                        {
                            #region 呼叫此 API，回傳一個 網頁 對不起，未能找到您要的網頁。可能該網頁已被移除或被移到其他的網址
                            APIResult apiResult = new APIResult()
                            {
                                Status = false,
                                ErrorCode = (int)ErrorMessageEnum.客製化文字錯誤訊息,
                                HTTPStatus = StatusCodes.Status404NotFound,
                                Message = "對不起，未能找到您要的網頁。可能該網頁已被移除或被移到其他的網址",
                                Payload = null
                            };
                            fakeResponseBody.Seek(0, SeekOrigin.Begin);
                            #region 清空此 Memory Stream
                            byte[] buffer = fakeResponseBody.GetBuffer();
                            Array.Clear(buffer, 0, buffer.Length);
                            fakeResponseBody.Position = 0;
                            fakeResponseBody.SetLength(0);
                            #endregion
                            string notFoundJson = JsonConvert.SerializeObject(apiResult);
                            streamWriter = new StreamWriter(fakeResponseBody);
                            await streamWriter.WriteLineAsync(notFoundJson);
                            await streamWriter.FlushAsync();
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            #endregion
                        }
                    }

                    fakeResponseBody.Seek(0, SeekOrigin.Begin);
                    await fakeResponseBody.CopyToAsync(originalBodyStream);
                    fakeResponseBody.Dispose();
                    streamWriter?.Dispose();
                }
            }
        }
    }

    public static class ApiNotFoundPageToApiResultMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiNotFoundPageToApiResult(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiNotFoundPageToApiResultMiddleware>();
        }
    }
}
