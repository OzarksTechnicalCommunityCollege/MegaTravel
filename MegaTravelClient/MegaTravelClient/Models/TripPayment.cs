namespace MegaTravelClient.Models
{
    public class TripPayment
    {
        public int PaymentId { get; set; }
        public int TripId { get; set; }
        public bool PaymentStatus { get; set; }
        public virtual TripData tripPaid { get; set; } = null!;
    }
}
