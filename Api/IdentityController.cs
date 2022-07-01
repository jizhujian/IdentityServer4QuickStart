using Microsoft.AspNetCore.Mvc;

namespace Api
{
    /// <summary>
    /// 标识
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        /// <summary>
        /// 用户声明
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserClaims()
        {
            return Ok(User.Claims.Select(it => new { it.Type, it.Value }).ToList());
        }
    }
}

