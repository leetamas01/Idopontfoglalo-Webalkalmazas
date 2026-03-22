namespace IdopontfoglaloWebalk.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password_Hash { get; set; }
        public string? role { get; set; }
        public double rating { get; set; }
        public string? service  { get; set; }

    }
}
