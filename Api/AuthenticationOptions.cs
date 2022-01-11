namespace Api
{
    public class AuthenticationOptions
    {
        public string Authority { get; set; } = string.Empty;
        public bool RequireHttpsMetadata { get; set; }
        public string Scope { get; set; } = string.Empty;
    }
}
