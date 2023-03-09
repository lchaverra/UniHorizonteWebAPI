using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace UniHorizonte_Web_API.Controllers {
  [EnableCors("AllowAnyOrigins")]
  public class SignInController : ControllerBase {

    [HttpPost]
    public IActionResult SignIn() {
      return Ok();
    }
  }
}
