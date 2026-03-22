namespace IdopontfoglaloWebalk.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime date {  get; set; }
        public DateTime Reservation_date { get; set; }
        public string? status { get; set; }
    }
}
