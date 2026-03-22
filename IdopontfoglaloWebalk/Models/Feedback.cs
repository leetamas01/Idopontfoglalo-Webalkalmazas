namespace IdopontfoglaloWebalk.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Reservation_Id { get; set; }
        public int Service_Owner_Id { get; set; }
        public int Guest_Id { get; set; }
        public string? Feedbacktext_Owner { get; set; }
        public string? Feedbacktext_Guest { get; set; }
        public string? Rating_Owner { get; set; }
        public string? Rating_Guest { get; set; }
    }
}
