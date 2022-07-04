namespace MvcClient
{
    public class AuthenticationOptions
    {
        public string Authority { get; set; } = string.Empty;
        public bool RequireHttpsMetadata { get; set; }
        public string[] Scopes { get; set; } = Array.Empty<string>();
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
