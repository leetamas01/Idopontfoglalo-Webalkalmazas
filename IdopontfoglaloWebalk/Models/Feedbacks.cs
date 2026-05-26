namespace IdopontfoglaloWebalk.Models
{
    public class Feedbacks
    {
        public int feedback_id { get; set; }
        public int reservation_Id { get; set; }
        public Occasions? Reservation { get; set; }
        public string? service_owner_id { get; set; }
        public Users? ServiceOwner { get; set; }

        public string? guest_id { get; set; }
        public Users? Guest { get; set; }

        public string? feedbacktext_owner { get; set; }
        public string? feedbacktext_guest { get; set; }

        public double rating_owner { get; set; }
        public double rating_guest { get; set; }
    }
}
