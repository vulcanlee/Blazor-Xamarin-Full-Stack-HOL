﻿using Backend.AdapterModels;
using Backend.Services;
using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShareBusiness.Factories;
using ShareBusiness.Helpers;
using ShareDomain.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly IMyUserService myUserService;

        public LoginController(Microsoft.Extensions.Configuration.IConfiguration configuration,
            IMyUserService myUserService)
        {
            this.configuration = configuration;
            this.myUserService = myUserService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(LoginRequestDto loginRequestDTO)
        {
            APIResult apiResult;
            await Task.Yield();
            if (ModelState.IsValid == false)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status200OK,
                ErrorMessageEnum.傳送過來的資料有問題);
                return Ok(apiResult);
            }

            (MyUserAdapterModel user, string message) = await myUserService.CheckUser(loginRequestDTO.Account, loginRequestDTO.Password);

            if (user == null)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status400BadRequest,
                ErrorMessageEnum.帳號或密碼不正確);
                return BadRequest(apiResult);
            }

            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToken(user);

            LoginResponseDto LoginResponseDTO = new LoginResponseDto()
            {
                Account = loginRequestDTO.Account,
                Id = user.Id,
                Name = loginRequestDTO.Account,
                Token = token,
                TokenExpireMinutes = Convert.ToInt32(configuration["Tokens:JwtExpireMinutes"]),
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = Convert.ToInt32(configuration["Tokens:JwtRefreshExpireDays"]),
            };

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
               ErrorMessageEnum.None, payload: LoginResponseDTO);
            return Ok(apiResult);

        }

        [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "RefreshToken")]
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            APIResult apiResult;
            await Task.Yield();
            LoginRequestDto loginRequestDTO = new LoginRequestDto()
            {
                Account = User.FindFirst(ClaimTypes.Sid)?.Value,
            };

            MyUserAdapterModel user = await myUserService.GetAsync(Convert.ToInt32(loginRequestDTO.Account));
            if (user.Id == 0)
            {
                apiResult = APIResultFactory.Build(false, StatusCodes.Status401Unauthorized,
                ErrorMessageEnum.沒有發現指定的該使用者資料);
                return BadRequest(apiResult);
            }

            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToken(user);

            LoginResponseDto LoginResponseDTO = new LoginResponseDto()
            {
                Account = loginRequestDTO.Account,
                Id = 0,
                Name = loginRequestDTO.Account,
                Token = token,
                TokenExpireMinutes = Convert.ToInt32(configuration["Tokens:JwtExpireMinutes"]),
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = Convert.ToInt32(configuration["Tokens:JwtRefreshExpireDays"]),
            };

            apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
               ErrorMessageEnum.None, payload: LoginResponseDTO);
            return Ok(apiResult);

        }

        string GenerateToken(MyUserAdapterModel user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),

            };
            if (user.IsManager == true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Tokens:JwtExpireMinutes"])),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }

        string GenerateRefreshToken(MyUserAdapterModel user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, $"RefreshToken"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["Tokens:JwtRefreshExpireDays"])),
                //expires: DateTime.Now.AddMinutes(1),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(configuration["Tokens:IssuerSigningKey"])),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
    }
}
