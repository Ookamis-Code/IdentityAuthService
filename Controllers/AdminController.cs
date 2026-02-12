using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    //only admins
    [HttpGet("vault"), Authorize(Roles = "Admin")]
    public ActionResult<string> GetSecretData()
    {
        return Ok("Welcome to the Admin Vault.");
    }
    [HttpGet("lobby"), Authorize]
    public ActionResult<string> GetLobbyData()
    {
        return Ok("Welcome to the Lobby. Any registered user can see this.");
    }
}