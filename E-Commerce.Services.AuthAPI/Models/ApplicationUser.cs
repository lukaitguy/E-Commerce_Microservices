using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
