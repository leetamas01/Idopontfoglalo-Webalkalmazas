using Microsoft.AspNetCore.Identity;

namespace IdopontfoglaloWebalk.Models
{
    public class User : IdentityUser
    {
        public int rating { get; set; }
        public string? service { get; set; }
        public bool rememberMe { get; set; }
    }
}