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
    public class ApiNotFoundPageToAPIResultMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiNotFoundPageToAPIResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            string responseContent;
            StreamWriter streamWriter = null;
            string keyUrl = "/api/";
            var originalStream = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                if (context.Request.Path.HasValue)
                {
                    if (context.Request.Path.Value.Length > keyUrl.Length &&
                        context.Request.Path.Value.ToLower().Substring(0, keyUrl.Length) == keyUrl)
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var reader = new StreamReader(memoryStream);
                        responseContent = await reader.ReadToEndAsync();
                        if (responseContent.Contains("<!DOCTYPE html>"))
                        {
                            #region 呼叫此 API，回傳一個 網頁 對不起，未能找到您要的網頁。可能該網頁已被移除或被移到其他的網址
                            var url = $"{context.Request.Scheme}://{context.Request.Host.Value}" +
                                $"{context.Request.Path.Value}";
                            APIResult apiResult = new APIResult()
                            {
                                Status = false,
                                ErrorCode = (int)ErrorMessageEnum.客製化文字錯誤訊息,
                                HTTPStatus = StatusCodes.Status404NotFound,
                                Message = $"對不起，未能找到您要的網頁。{url}",
                                Payload = null
                            };
                            memoryStream.Seek(0, SeekOrigin.Begin);

                            #region 清空此 Memory Stream
                            byte[] buffer = memoryStream.GetBuffer();
                            Array.Clear(buffer, 0, buffer.Length);
                            memoryStream.Position = 0;
                            memoryStream.SetLength(0);
                            #endregion

                            #region 將 Response.Body 替換為 APIResult JSON 內容
                            string notFoundJsonContent = JsonConvert.SerializeObject(apiResult);
                            streamWriter = new StreamWriter(memoryStream);
                            await streamWriter.WriteLineAsync(notFoundJsonContent);
                            await streamWriter.FlushAsync();
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            #endregion
                            #endregion
                        }
                    }

                    #region 將處理完的 Response.Body 複製到 context.Response.Body
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalStream);
                    streamWriter?.Dispose();
                    #endregion
                }
            }
        }
    }

    public static class ApiNotFoundPageToAPIResultMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiNotFoundPageToAPIResult(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiNotFoundPageToAPIResultMiddleware>();
        }
    }
}
