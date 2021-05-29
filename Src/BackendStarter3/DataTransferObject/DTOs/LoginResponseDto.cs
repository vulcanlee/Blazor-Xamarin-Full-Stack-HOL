using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObject.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public int TokenExpireMinutes { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }
}
