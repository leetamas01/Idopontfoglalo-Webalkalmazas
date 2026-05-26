using System.ComponentModel.DataAnnotations;

namespace IdopontfoglaloWebalk.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Az Email cím megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Nem megfelelő email formátum!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}