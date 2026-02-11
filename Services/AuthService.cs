using System.Security.Cryptography;
using System.Text;
using IdentityAuthService.Models;

namespace IdentityAuthService.Services;

public class AuthService
{
    public void CreatePasswordHash(string password, out byte[] password_Hash, out byte[] password_Salt)
    {
        using var hmac = new HMACSHA512();
        password_Salt = hmac.Key;
        password_Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    public bool VerifyPasswordHash(string password, byte[] stored_Hash, byte[] stored_Salt)
    {
        using var hmac = new HMACSHA512(stored_Salt);
        var computed_Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computed_Hash.SequenceEqual(stored_Hash);
    }
}