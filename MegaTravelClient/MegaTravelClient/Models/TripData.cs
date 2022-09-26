namespace MegaTravelClient.Models
{
    public class TripData
    {
        public int TripId { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public string TripName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public virtual Agent Agent { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual TripPayment PaymentStatus { get; set; } = null!;



    }
}
