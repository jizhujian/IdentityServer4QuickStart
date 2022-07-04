namespace ConsoleClient
{
    public class AuthenticationOptions
    {
        public string Authority { get; set; } = string.Empty;
        public bool RequireHttpsMetadata { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Scopes { get; set; } = string.Empty;
    }
}
