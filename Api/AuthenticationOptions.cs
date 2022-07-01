namespace Api
{
    /// <summary>
    /// API身份验证配置
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// OpenIdConnect服务器
        /// </summary>
        public string Authority { get; set; } = string.Empty;
        /// <summary>
        /// 是否需要HTTPS
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }
        /// <summary>
        /// API资源
        /// </summary>
        public string Scope { get; set; } = string.Empty;
    }
}
