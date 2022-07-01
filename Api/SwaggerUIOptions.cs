namespace Api
{
    /// <summary>
    /// SwggerUI配置
    /// </summary>
    public class SwaggerUIOptions
    {
        /// <summary>
        /// Api名称
        /// </summary>
        public string ApiName { get; set; } = string.Empty;
        /// <summary>
        /// Api版本
        /// </summary>
        public string ApiVersion { get; set; } = string.Empty;
        /// <summary>
        /// Api基URL
        /// </summary>
        public string ApiBaseUrl { get; set; } = string.Empty;
        /// <summary>
        /// OAuth客户端
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// OAuth客户端密钥
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
