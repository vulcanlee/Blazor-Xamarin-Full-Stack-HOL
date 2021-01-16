using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using DataTransferObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareBusiness.Factories;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Backend.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Backend.AdapterModels;
using ShareDomain.Enums;
using ShareBusiness.Helpers;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
        private readonly IHoluserService holuserService;

        public LoginController(Microsoft.Extensions.Configuration.IConfiguration configuration,
            IHoluserService holuserService)
        {
            this.configuration = configuration;
            this.holuserService = holuserService;
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

            (HoluserAdapterModel user, string message) = await holuserService.CheckUser(loginRequestDTO.Account, loginRequestDTO.Password);

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

        [Authorize(AuthenticationSchemes = MagicHelper.JwtBearerAuthenticationScheme, Roles = "RefreshToken")]
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            APIResult apiResult;
            await Task.Yield();
            LoginRequestDto loginRequestDTO = new LoginRequestDto()
            {
                Account = User.FindFirst(JwtRegisteredClaimNames.Sid)?.Value,
            };

            HoluserAdapterModel user = await holuserService.GetAsync(Convert.ToInt32(loginRequestDTO.Account));
            if (user == null)
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

        public string GenerateToken(HoluserAdapterModel user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("TokenVersion", user.TokenVersion.ToString()),
            };
            if (user.Level == 4)
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

        public string GenerateRefreshToken(HoluserAdapterModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, $"RefreshToken"),
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Tokens:ValidIssuer"],
                audience: configuration["Tokens:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(configuration["Tokens:JwtRefreshExpireDays"])),
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
