namespace Backend.Models
{
    public class BackendTokenConfiguration
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int JwtExpireMinutes { get; set; }
        public int JwtRefreshExpireDays { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}
