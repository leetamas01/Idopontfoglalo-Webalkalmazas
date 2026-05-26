using Microsoft.AspNetCore.Identity;

namespace IdopontfoglaloWebalk.Models
{
    public class Users : IdentityUser
    {
        public double rating { get; set; }
    }
}