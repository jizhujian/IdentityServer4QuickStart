using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api
{
  [Route("api/[controller]/[action]")]
  [Authorize]
  public class IdentityController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetUserClaims()
    {
      return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }
  }
}

