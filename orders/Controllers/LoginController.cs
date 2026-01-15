using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Unauthorized();
            }
            HttpContext.Session.SetString("Username", request.Email);
            return Ok(new { Token = "jwt-token" });
        }
        [HttpPost] public IActionResult Logout() { 
            HttpContext.Session.Remove("Username"); 
            return Ok("Logged out"); 
        }
    }
}
