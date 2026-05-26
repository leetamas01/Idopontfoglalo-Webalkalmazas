namespace IdopontfoglaloWebalk.Models
{
    public class ServiceCategories
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int service_id { get; set; }
        public virtual Services? Service { get; set; }
        public List<Occasions> Occasions { get; set; } = new();
    }
}
