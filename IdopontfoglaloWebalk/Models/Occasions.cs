namespace IdopontfoglaloWebalk.Models
{
    public class Occasions
    {
        public int reservation_id { get; set; }
        public int service_category_id { get; set; }
        public ServiceCategories? ServiceCategory { get; set; }

        public string? user_id { get; set; } 
        public Users? User { get; set; }

        public DateTime date { get; set; }
        public DateTime reservation_date { get; set; }
        public string? status { get; set; }
    }
}
