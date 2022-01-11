using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserClaims()
        {
            return Ok((from c in User.Claims select new { c.Type, c.Value }).ToList());
        }
    }
}

