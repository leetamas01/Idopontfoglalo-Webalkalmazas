using System.Collections.Generic;

namespace IdopontfoglaloWebalk.Models
{
    public class ProfileViewModel
    {
        public Users User { get; set; } = null!;
        public List<Occasions> Reservations { get; set; } = new();
    }
}