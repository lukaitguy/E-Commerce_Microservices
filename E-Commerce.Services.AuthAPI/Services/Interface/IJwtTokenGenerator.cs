using E_Commerce.Services.AuthAPI.Models;

namespace E_Commerce.Services.AuthAPI.Services.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}
