namespace MegaTravelClient.Models
{
    public class TripPaymentResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TripPayment payment { get; set; }
    }
}
