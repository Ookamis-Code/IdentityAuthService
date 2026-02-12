using Microsoft.AspNetCore.Mvc;
using IdentityAuthService.Models;
using IdentityAuthService.Services;

namespace IdentityAuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private static List<User> p_users = new();
    private readonly AuthService auth_Service;
    private readonly JwtService jwt_Serivce;
    public AuthController(AuthService authService, JwtService jwtService)
    {
        auth_Service = authService;
        jwt_Serivce = jwtService;
    }
    [HttpPost("register")]
    public ActionResult<User> Register(string username, string password, string role = "User")
    {
        var valid_Roles = new List<string> { "User", "Admin", "Manager" };
        if (!valid_Roles.Contains(role)) return BadRequest("Invalid Role.");
        auth_Service.CreatePasswordHash(password, out byte[] hash, out byte[] salt);
        var user = new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = role
        };
        p_users.Add(user);
        return Ok("User registration successful.");
    }
    [HttpPost("login")]
    public ActionResult<string> Login(string username, string password)
    {
        //locate user
        var user = p_users.FirstOrDefault(u => u.Username == username);
        if (user == null) return BadRequest("User not found.");
        //confirm handshake, pref vanilla
        if (!auth_Service.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }
        //give pass
        string token = jwt_Serivce.CreateToken(user);
        return Ok(token);
    }
}