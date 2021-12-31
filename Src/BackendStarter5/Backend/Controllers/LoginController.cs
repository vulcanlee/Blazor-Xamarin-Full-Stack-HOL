using Backend.AdapterModels;
using Backend.Models;
using Backend.Services;
using DTOs.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BAL.Factories;
using BAL.Helpers;
using CommonDomain.Enums;
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
        private readonly BackendTokenConfiguration tokenConfiguration;

        public LoginController(Microsoft.Extensions.Configuration.IConfiguration configuration,
            IMyUserService myUserService, IOptions<BackendTokenConfiguration> tokenConfiguration)
        {
            this.configuration = configuration;
            this.myUserService = myUserService;
            this.tokenConfiguration = tokenConfiguration.Value;
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
                TokenExpireMinutes = tokenConfiguration.JwtExpireMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = tokenConfiguration.JwtRefreshExpireDays,
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
                TokenExpireMinutes = tokenConfiguration.JwtExpireMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpireDays = tokenConfiguration.JwtRefreshExpireDays,
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

            var token = new JwtSecurityToken
            (
                issuer: tokenConfiguration.ValidIssuer,
                audience: tokenConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenConfiguration.JwtExpireMinutes),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(tokenConfiguration.IssuerSigningKey)),
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
                issuer: tokenConfiguration.ValidIssuer,
                audience: tokenConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(tokenConfiguration.JwtRefreshExpireDays),
                //expires: DateTime.Now.AddMinutes(1),
                //notBefore: DateTime.Now.AddMinutes(-5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(tokenConfiguration.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha512)
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
    }
}
